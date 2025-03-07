﻿using OpenBots.Core.Command;
using OpenBots.Core.Interfaces;
using OpenBots.Core.Properties;
using OpenBots.Core.Script;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Tasks = System.Threading.Tasks;

namespace OpenBots.Commands.ErrorHandling
{
    [Serializable]
    [Category("Error Handling Commands")]
    [Description("This command defines a try/catch block which will execute the associated catch block if any " +
                 "exceptions are thrown.")]
    public class BeginTryCommand : ScriptCommand
    {      
        public BeginTryCommand()
        {
            CommandName = "BeginTryCommand";
            SelectionName = "Try";
            CommandEnabled = true;
            CommandIcon = Resources.command_try;
            ScopeStartCommand = true;
        }

        public async override Tasks.Task RunCommand(object sender, ScriptAction parentCommand)
        {
            //get engine
            var engine = (IAutomationEngineInstance) sender;

            //get indexes of commands
            var startIndex = 0;
            var catchIndices = FindAllCatchIndices(parentCommand.AdditionalScriptCommands);
            var firstCatchIndex = catchIndices.First();
            var startFinallyIndex = parentCommand.AdditionalScriptCommands.FindIndex(a => a.ScriptCommand is FinallyCommand);
            var endTryIndex = parentCommand.AdditionalScriptCommands.FindIndex(a => a.ScriptCommand is EndTryCommand);

            for (var tryIndex = startIndex; tryIndex < firstCatchIndex; tryIndex++)
            {
                if (engine.IsCancellationPending || engine.CurrentLoopCancelled)
                    return;

                try
                {
                    var cmd = parentCommand.AdditionalScriptCommands[tryIndex];
                    cmd.IsExceptionIgnored = true;
                    await engine.ExecuteCommand(cmd);
                    if(cmd.ScriptCommand.CommandName == "RunTaskCommand" && engine.ChildScriptFailed && !engine.ChildScriptErrorCaught)
                        throw new Exception("Child Script Failed");
                }
                catch (Exception ex)
                {
                    var targetCatchIndex = -1;
                    var generalCatchIndex = -1;
                    var exceptionType = ex.GetType().Name;
                    ScriptAction catchCommandItem;
                    CatchCommand targetCatchCommand;

                    if(engine.ChildScriptFailed)
                    {
                        engine.ChildScriptErrorCaught = true;
                        exceptionType = engine.ErrorsOccured.OrderByDescending(x => x.LineNumber).FirstOrDefault().ErrorType;
                    }

                    // get index of target catch
                    foreach (var catchIndex in catchIndices)
                    {
                        catchCommandItem = parentCommand.AdditionalScriptCommands[catchIndex];
                        targetCatchCommand = (CatchCommand)catchCommandItem.ScriptCommand;

                        // Save Generic Exception Catch Index (If found)
                        if (targetCatchCommand.v_ExceptionType.ToLower() == "exception")
                        {
                            generalCatchIndex = catchIndex;
                        }
                        // If the type of the Exception (occurred) matches with any of the Exception Types (in Automation Script)
                        if (exceptionType.ToLower() == targetCatchCommand.v_ExceptionType.ToLower())
                        {
                            targetCatchIndex = catchIndex;
                            break;
                        }
                    }

                    // Index of Finally or EndTry (Where a Target Catch Block Ends)
                    var endCatch = startFinallyIndex != -1 ? startFinallyIndex : endTryIndex;

                    // If Target Catch Found
                    if (targetCatchIndex != -1)
                    {
                        await ExecuteTargetCatchBlock(sender, parentCommand, targetCatchIndex, endCatch);
                    }
                    // Else If Generic Exception Catch Found
                    else if(generalCatchIndex != -1)
                    {
                        await ExecuteTargetCatchBlock(sender, parentCommand, generalCatchIndex, endCatch);
                    }
                    else
                    {
                        throw new Exception("No Valid Catch found.");
                    }

                    break;
                }
            }

            //handle finally block if exists
            if (startFinallyIndex != -1)
            {
                for (var finallyIndex = startFinallyIndex; finallyIndex < endTryIndex; finallyIndex++)
                {
                    await engine.ExecuteCommand(parentCommand.AdditionalScriptCommands[finallyIndex]);
                }
            }
            // If Catch block executes smoothly
            engine.ErrorsOccured.Clear();
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue();
        }

        private List<int> FindAllCatchIndices(List<ScriptAction> additionalCommands)
        {
            // get the count of all (Enabled) Catch Commands
            int totalCatchCommands = additionalCommands.FindAll(
                action => action.ScriptCommand is CatchCommand && 
                action.ScriptCommand.IsCommented == false
                ).Count;

            if (totalCatchCommands == 0)
            {
                return null;
            }
            else
            {
                List<int> catchIndices = new List<int>();
                int startIndex = 0;

                // get the indices of all (Enabled) Catch Commands
                while (startIndex < additionalCommands.Count && (startIndex = additionalCommands.FindIndex(
                    startIndex, (a => a.ScriptCommand is CatchCommand && a.ScriptCommand.IsCommented == false))) != -1)
                {
                    catchIndices.Add(startIndex++);
                }

                return catchIndices;
            }
        }

        private int FindNextCatchIndex(List<int> catches, int currCatch)
        {
            int nextCatch;
            var currentCatchIndex = catches.IndexOf(currCatch);

            try
            {
                nextCatch = catches[currentCatchIndex + 1];
            }
            catch(Exception)
            {
                nextCatch = currCatch;
            }

            return nextCatch;
        }

        private async Tasks.Task ExecuteTargetCatchBlock(object sender, ScriptAction parentCommand, int startCatchIndex, int endCatchIndex)
        {
            //get engine
            var engine = (IAutomationEngineInstance)sender;

            //reset exception flags
            engine.ChildScriptFailed = false;
            engine.ChildScriptErrorCaught = false;

            var catchIndices = FindAllCatchIndices(parentCommand.AdditionalScriptCommands);

            // Next Catch Index
            var nextCatchIndex = FindNextCatchIndex(catchIndices, startCatchIndex);

            // If Next Catch Exist
            if (nextCatchIndex != startCatchIndex)
            {
                // Next Catch will be the end of the Target Catch
                endCatchIndex = nextCatchIndex;
            }

            // Execute Target Catch Block
            for (var catchIndex = startCatchIndex; catchIndex < endCatchIndex; catchIndex++)
            {
                await engine.ExecuteCommand(parentCommand.AdditionalScriptCommands[catchIndex]);
            }
        }
    }
}