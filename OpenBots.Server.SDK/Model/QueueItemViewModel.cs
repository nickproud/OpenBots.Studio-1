/* 
 * OpenBots Server API
 *
 * No description provided (generated by Swagger Codegen https://github.com/swagger-api/swagger-codegen)
 *
 * OpenAPI spec version: v1
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */
using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;
using SwaggerDateConverter = OpenBots.Server.SDK.Client.SwaggerDateConverter;

namespace OpenBots.Server.SDK.Model
{
    /// <summary>
    /// QueueItemViewModel
    /// </summary>
    [DataContract]
        public partial class QueueItemViewModel :  IEquatable<QueueItemViewModel>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueueItemViewModel" /> class.
        /// </summary>
        /// <param name="id">id.</param>
        /// <param name="isDeleted">isDeleted (default to false).</param>
        /// <param name="createdBy">createdBy.</param>
        /// <param name="createdOn">createdOn.</param>
        /// <param name="deletedBy">deletedBy.</param>
        /// <param name="deleteOn">deleteOn.</param>
        /// <param name="timestamp">timestamp.</param>
        /// <param name="updatedOn">updatedOn.</param>
        /// <param name="updatedBy">updatedBy.</param>
        /// <param name="name">name (required).</param>
        /// <param name="state">state.</param>
        /// <param name="stateMessage">stateMessage.</param>
        /// <param name="isLocked">isLocked.</param>
        /// <param name="lockedBy">lockedBy.</param>
        /// <param name="lockedOnUTC">lockedOnUTC.</param>
        /// <param name="lockedUntilUTC">lockedUntilUTC.</param>
        /// <param name="lockedEndTimeUTC">lockedEndTimeUTC.</param>
        /// <param name="expireOnUTC">expireOnUTC.</param>
        /// <param name="postponeUntilUTC">postponeUntilUTC.</param>
        /// <param name="errorCode">errorCode.</param>
        /// <param name="errorMessage">errorMessage.</param>
        /// <param name="errorSerialized">errorSerialized.</param>
        /// <param name="source">source.</param>
        /// <param name="_event">_event.</param>
        /// <param name="resultJSON">resultJSON.</param>
        /// <param name="queueId">queueId.</param>
        /// <param name="type">type.</param>
        /// <param name="jsonType">jsonType.</param>
        /// <param name="dataJson">dataJson.</param>
        /// <param name="lockTransactionKey">lockTransactionKey.</param>
        /// <param name="retryCount">retryCount.</param>
        /// <param name="priority">priority.</param>
        /// <param name="payloadSizeInBytes">payloadSizeInBytes.</param>
        /// <param name="attachments">attachments.</param>
        public QueueItemViewModel(Guid? id = default(Guid?), bool? isDeleted = false, string createdBy = default(string), DateTime? createdOn = default(DateTime?), string deletedBy = default(string), DateTime? deleteOn = default(DateTime?), byte[] timestamp = default(byte[]), DateTime? updatedOn = default(DateTime?), string updatedBy = default(string), string name = default(string), string state = default(string), string stateMessage = default(string), bool? isLocked = default(bool?), Guid? lockedBy = default(Guid?), DateTime? lockedOnUTC = default(DateTime?), DateTime? lockedUntilUTC = default(DateTime?), DateTime? lockedEndTimeUTC = default(DateTime?), DateTime? expireOnUTC = default(DateTime?), DateTime? postponeUntilUTC = default(DateTime?), string errorCode = default(string), string errorMessage = default(string), string errorSerialized = default(string), string source = default(string), string _event = default(string), string resultJSON = default(string), Guid? queueId = default(Guid?), string type = default(string), string jsonType = default(string), string dataJson = default(string), Guid? lockTransactionKey = default(Guid?), int? retryCount = default(int?), int? priority = default(int?), long? payloadSizeInBytes = default(long?), List<QueueItemAttachment> attachments = default(List<QueueItemAttachment>))
        {
            // to ensure "name" is required (not null)
            if (name == null)
            {
                throw new InvalidDataException("name is a required property for QueueItemViewModel and cannot be null");
            }
            else
            {
                this.Name = name;
            }
            this.Id = id;
            // use default value if no "isDeleted" provided
            if (isDeleted == null)
            {
                this.IsDeleted = false;
            }
            else
            {
                this.IsDeleted = isDeleted;
            }
            this.CreatedBy = createdBy;
            this.CreatedOn = createdOn;
            this.DeletedBy = deletedBy;
            this.DeleteOn = deleteOn;
            this.Timestamp = timestamp;
            this.UpdatedOn = updatedOn;
            this.UpdatedBy = updatedBy;
            this.State = state;
            this.StateMessage = stateMessage;
            this.IsLocked = isLocked;
            this.LockedBy = lockedBy;
            this.LockedOnUTC = lockedOnUTC;
            this.LockedUntilUTC = lockedUntilUTC;
            this.LockedEndTimeUTC = lockedEndTimeUTC;
            this.ExpireOnUTC = expireOnUTC;
            this.PostponeUntilUTC = postponeUntilUTC;
            this.ErrorCode = errorCode;
            this.ErrorMessage = errorMessage;
            this.ErrorSerialized = errorSerialized;
            this.Source = source;
            this.Event = _event;
            this.ResultJSON = resultJSON;
            this.QueueId = queueId;
            this.Type = type;
            this.JsonType = jsonType;
            this.DataJson = dataJson;
            this.LockTransactionKey = lockTransactionKey;
            this.RetryCount = retryCount;
            this.Priority = priority;
            this.PayloadSizeInBytes = payloadSizeInBytes;
            this.Attachments = attachments;
        }
        
        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name="id", EmitDefaultValue=false)]
        public Guid? Id { get; set; }

        /// <summary>
        /// Gets or Sets IsDeleted
        /// </summary>
        [DataMember(Name="isDeleted", EmitDefaultValue=false)]
        public bool? IsDeleted { get; set; }

        /// <summary>
        /// Gets or Sets CreatedBy
        /// </summary>
        [DataMember(Name="createdBy", EmitDefaultValue=false)]
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or Sets CreatedOn
        /// </summary>
        [DataMember(Name="createdOn", EmitDefaultValue=false)]
        public DateTime? CreatedOn { get; set; }

        /// <summary>
        /// Gets or Sets DeletedBy
        /// </summary>
        [DataMember(Name="deletedBy", EmitDefaultValue=false)]
        public string DeletedBy { get; set; }

        /// <summary>
        /// Gets or Sets DeleteOn
        /// </summary>
        [DataMember(Name="deleteOn", EmitDefaultValue=false)]
        public DateTime? DeleteOn { get; set; }

        /// <summary>
        /// Gets or Sets Timestamp
        /// </summary>
        [DataMember(Name="timestamp", EmitDefaultValue=false)]
        public byte[] Timestamp { get; set; }

        /// <summary>
        /// Gets or Sets UpdatedOn
        /// </summary>
        [DataMember(Name="updatedOn", EmitDefaultValue=false)]
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// Gets or Sets UpdatedBy
        /// </summary>
        [DataMember(Name="updatedBy", EmitDefaultValue=false)]
        public string UpdatedBy { get; set; }

        /// <summary>
        /// Gets or Sets Name
        /// </summary>
        [DataMember(Name="name", EmitDefaultValue=false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets State
        /// </summary>
        [DataMember(Name="state", EmitDefaultValue=false)]
        public string State { get; set; }

        /// <summary>
        /// Gets or Sets StateMessage
        /// </summary>
        [DataMember(Name="stateMessage", EmitDefaultValue=false)]
        public string StateMessage { get; set; }

        /// <summary>
        /// Gets or Sets IsLocked
        /// </summary>
        [DataMember(Name="isLocked", EmitDefaultValue=false)]
        public bool? IsLocked { get; set; }

        /// <summary>
        /// Gets or Sets LockedBy
        /// </summary>
        [DataMember(Name="lockedBy", EmitDefaultValue=false)]
        public Guid? LockedBy { get; set; }

        /// <summary>
        /// Gets or Sets LockedOnUTC
        /// </summary>
        [DataMember(Name="lockedOnUTC", EmitDefaultValue=false)]
        public DateTime? LockedOnUTC { get; set; }

        /// <summary>
        /// Gets or Sets LockedUntilUTC
        /// </summary>
        [DataMember(Name="lockedUntilUTC", EmitDefaultValue=false)]
        public DateTime? LockedUntilUTC { get; set; }

        /// <summary>
        /// Gets or Sets LockedEndTimeUTC
        /// </summary>
        [DataMember(Name="lockedEndTimeUTC", EmitDefaultValue=false)]
        public DateTime? LockedEndTimeUTC { get; set; }

        /// <summary>
        /// Gets or Sets ExpireOnUTC
        /// </summary>
        [DataMember(Name="expireOnUTC", EmitDefaultValue=false)]
        public DateTime? ExpireOnUTC { get; set; }

        /// <summary>
        /// Gets or Sets PostponeUntilUTC
        /// </summary>
        [DataMember(Name="postponeUntilUTC", EmitDefaultValue=false)]
        public DateTime? PostponeUntilUTC { get; set; }

        /// <summary>
        /// Gets or Sets ErrorCode
        /// </summary>
        [DataMember(Name="errorCode", EmitDefaultValue=false)]
        public string ErrorCode { get; set; }

        /// <summary>
        /// Gets or Sets ErrorMessage
        /// </summary>
        [DataMember(Name="errorMessage", EmitDefaultValue=false)]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or Sets ErrorSerialized
        /// </summary>
        [DataMember(Name="errorSerialized", EmitDefaultValue=false)]
        public string ErrorSerialized { get; set; }

        /// <summary>
        /// Gets or Sets Source
        /// </summary>
        [DataMember(Name="source", EmitDefaultValue=false)]
        public string Source { get; set; }

        /// <summary>
        /// Gets or Sets Event
        /// </summary>
        [DataMember(Name="event", EmitDefaultValue=false)]
        public string Event { get; set; }

        /// <summary>
        /// Gets or Sets ResultJSON
        /// </summary>
        [DataMember(Name="resultJSON", EmitDefaultValue=false)]
        public string ResultJSON { get; set; }

        /// <summary>
        /// Gets or Sets QueueId
        /// </summary>
        [DataMember(Name="queueId", EmitDefaultValue=false)]
        public Guid? QueueId { get; set; }

        /// <summary>
        /// Gets or Sets Type
        /// </summary>
        [DataMember(Name="type", EmitDefaultValue=false)]
        public string Type { get; set; }

        /// <summary>
        /// Gets or Sets JsonType
        /// </summary>
        [DataMember(Name="jsonType", EmitDefaultValue=false)]
        public string JsonType { get; set; }

        /// <summary>
        /// Gets or Sets DataJson
        /// </summary>
        [DataMember(Name="dataJson", EmitDefaultValue=false)]
        public string DataJson { get; set; }

        /// <summary>
        /// Gets or Sets LockTransactionKey
        /// </summary>
        [DataMember(Name="lockTransactionKey", EmitDefaultValue=false)]
        public Guid? LockTransactionKey { get; set; }

        /// <summary>
        /// Gets or Sets RetryCount
        /// </summary>
        [DataMember(Name="retryCount", EmitDefaultValue=false)]
        public int? RetryCount { get; set; }

        /// <summary>
        /// Gets or Sets Priority
        /// </summary>
        [DataMember(Name="priority", EmitDefaultValue=false)]
        public int? Priority { get; set; }

        /// <summary>
        /// Gets or Sets PayloadSizeInBytes
        /// </summary>
        [DataMember(Name="payloadSizeInBytes", EmitDefaultValue=false)]
        public long? PayloadSizeInBytes { get; set; }

        /// <summary>
        /// Gets or Sets Attachments
        /// </summary>
        [DataMember(Name="attachments", EmitDefaultValue=false)]
        public List<QueueItemAttachment> Attachments { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class QueueItemViewModel {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  IsDeleted: ").Append(IsDeleted).Append("\n");
            sb.Append("  CreatedBy: ").Append(CreatedBy).Append("\n");
            sb.Append("  CreatedOn: ").Append(CreatedOn).Append("\n");
            sb.Append("  DeletedBy: ").Append(DeletedBy).Append("\n");
            sb.Append("  DeleteOn: ").Append(DeleteOn).Append("\n");
            sb.Append("  Timestamp: ").Append(Timestamp).Append("\n");
            sb.Append("  UpdatedOn: ").Append(UpdatedOn).Append("\n");
            sb.Append("  UpdatedBy: ").Append(UpdatedBy).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  State: ").Append(State).Append("\n");
            sb.Append("  StateMessage: ").Append(StateMessage).Append("\n");
            sb.Append("  IsLocked: ").Append(IsLocked).Append("\n");
            sb.Append("  LockedBy: ").Append(LockedBy).Append("\n");
            sb.Append("  LockedOnUTC: ").Append(LockedOnUTC).Append("\n");
            sb.Append("  LockedUntilUTC: ").Append(LockedUntilUTC).Append("\n");
            sb.Append("  LockedEndTimeUTC: ").Append(LockedEndTimeUTC).Append("\n");
            sb.Append("  ExpireOnUTC: ").Append(ExpireOnUTC).Append("\n");
            sb.Append("  PostponeUntilUTC: ").Append(PostponeUntilUTC).Append("\n");
            sb.Append("  ErrorCode: ").Append(ErrorCode).Append("\n");
            sb.Append("  ErrorMessage: ").Append(ErrorMessage).Append("\n");
            sb.Append("  ErrorSerialized: ").Append(ErrorSerialized).Append("\n");
            sb.Append("  Source: ").Append(Source).Append("\n");
            sb.Append("  Event: ").Append(Event).Append("\n");
            sb.Append("  ResultJSON: ").Append(ResultJSON).Append("\n");
            sb.Append("  QueueId: ").Append(QueueId).Append("\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
            sb.Append("  JsonType: ").Append(JsonType).Append("\n");
            sb.Append("  DataJson: ").Append(DataJson).Append("\n");
            sb.Append("  LockTransactionKey: ").Append(LockTransactionKey).Append("\n");
            sb.Append("  RetryCount: ").Append(RetryCount).Append("\n");
            sb.Append("  Priority: ").Append(Priority).Append("\n");
            sb.Append("  PayloadSizeInBytes: ").Append(PayloadSizeInBytes).Append("\n");
            sb.Append("  Attachments: ").Append(Attachments).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
  
        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input)
        {
            return this.Equals(input as QueueItemViewModel);
        }

        /// <summary>
        /// Returns true if QueueItemViewModel instances are equal
        /// </summary>
        /// <param name="input">Instance of QueueItemViewModel to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(QueueItemViewModel input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.Id == input.Id ||
                    (this.Id != null &&
                    this.Id.Equals(input.Id))
                ) && 
                (
                    this.IsDeleted == input.IsDeleted ||
                    (this.IsDeleted != null &&
                    this.IsDeleted.Equals(input.IsDeleted))
                ) && 
                (
                    this.CreatedBy == input.CreatedBy ||
                    (this.CreatedBy != null &&
                    this.CreatedBy.Equals(input.CreatedBy))
                ) && 
                (
                    this.CreatedOn == input.CreatedOn ||
                    (this.CreatedOn != null &&
                    this.CreatedOn.Equals(input.CreatedOn))
                ) && 
                (
                    this.DeletedBy == input.DeletedBy ||
                    (this.DeletedBy != null &&
                    this.DeletedBy.Equals(input.DeletedBy))
                ) && 
                (
                    this.DeleteOn == input.DeleteOn ||
                    (this.DeleteOn != null &&
                    this.DeleteOn.Equals(input.DeleteOn))
                ) && 
                (
                    this.Timestamp == input.Timestamp ||
                    (this.Timestamp != null &&
                    this.Timestamp.Equals(input.Timestamp))
                ) && 
                (
                    this.UpdatedOn == input.UpdatedOn ||
                    (this.UpdatedOn != null &&
                    this.UpdatedOn.Equals(input.UpdatedOn))
                ) && 
                (
                    this.UpdatedBy == input.UpdatedBy ||
                    (this.UpdatedBy != null &&
                    this.UpdatedBy.Equals(input.UpdatedBy))
                ) && 
                (
                    this.Name == input.Name ||
                    (this.Name != null &&
                    this.Name.Equals(input.Name))
                ) && 
                (
                    this.State == input.State ||
                    (this.State != null &&
                    this.State.Equals(input.State))
                ) && 
                (
                    this.StateMessage == input.StateMessage ||
                    (this.StateMessage != null &&
                    this.StateMessage.Equals(input.StateMessage))
                ) && 
                (
                    this.IsLocked == input.IsLocked ||
                    (this.IsLocked != null &&
                    this.IsLocked.Equals(input.IsLocked))
                ) && 
                (
                    this.LockedBy == input.LockedBy ||
                    (this.LockedBy != null &&
                    this.LockedBy.Equals(input.LockedBy))
                ) && 
                (
                    this.LockedOnUTC == input.LockedOnUTC ||
                    (this.LockedOnUTC != null &&
                    this.LockedOnUTC.Equals(input.LockedOnUTC))
                ) && 
                (
                    this.LockedUntilUTC == input.LockedUntilUTC ||
                    (this.LockedUntilUTC != null &&
                    this.LockedUntilUTC.Equals(input.LockedUntilUTC))
                ) && 
                (
                    this.LockedEndTimeUTC == input.LockedEndTimeUTC ||
                    (this.LockedEndTimeUTC != null &&
                    this.LockedEndTimeUTC.Equals(input.LockedEndTimeUTC))
                ) && 
                (
                    this.ExpireOnUTC == input.ExpireOnUTC ||
                    (this.ExpireOnUTC != null &&
                    this.ExpireOnUTC.Equals(input.ExpireOnUTC))
                ) && 
                (
                    this.PostponeUntilUTC == input.PostponeUntilUTC ||
                    (this.PostponeUntilUTC != null &&
                    this.PostponeUntilUTC.Equals(input.PostponeUntilUTC))
                ) && 
                (
                    this.ErrorCode == input.ErrorCode ||
                    (this.ErrorCode != null &&
                    this.ErrorCode.Equals(input.ErrorCode))
                ) && 
                (
                    this.ErrorMessage == input.ErrorMessage ||
                    (this.ErrorMessage != null &&
                    this.ErrorMessage.Equals(input.ErrorMessage))
                ) && 
                (
                    this.ErrorSerialized == input.ErrorSerialized ||
                    (this.ErrorSerialized != null &&
                    this.ErrorSerialized.Equals(input.ErrorSerialized))
                ) && 
                (
                    this.Source == input.Source ||
                    (this.Source != null &&
                    this.Source.Equals(input.Source))
                ) && 
                (
                    this.Event == input.Event ||
                    (this.Event != null &&
                    this.Event.Equals(input.Event))
                ) && 
                (
                    this.ResultJSON == input.ResultJSON ||
                    (this.ResultJSON != null &&
                    this.ResultJSON.Equals(input.ResultJSON))
                ) && 
                (
                    this.QueueId == input.QueueId ||
                    (this.QueueId != null &&
                    this.QueueId.Equals(input.QueueId))
                ) && 
                (
                    this.Type == input.Type ||
                    (this.Type != null &&
                    this.Type.Equals(input.Type))
                ) && 
                (
                    this.JsonType == input.JsonType ||
                    (this.JsonType != null &&
                    this.JsonType.Equals(input.JsonType))
                ) && 
                (
                    this.DataJson == input.DataJson ||
                    (this.DataJson != null &&
                    this.DataJson.Equals(input.DataJson))
                ) && 
                (
                    this.LockTransactionKey == input.LockTransactionKey ||
                    (this.LockTransactionKey != null &&
                    this.LockTransactionKey.Equals(input.LockTransactionKey))
                ) && 
                (
                    this.RetryCount == input.RetryCount ||
                    (this.RetryCount != null &&
                    this.RetryCount.Equals(input.RetryCount))
                ) && 
                (
                    this.Priority == input.Priority ||
                    (this.Priority != null &&
                    this.Priority.Equals(input.Priority))
                ) && 
                (
                    this.PayloadSizeInBytes == input.PayloadSizeInBytes ||
                    (this.PayloadSizeInBytes != null &&
                    this.PayloadSizeInBytes.Equals(input.PayloadSizeInBytes))
                ) && 
                (
                    this.Attachments == input.Attachments ||
                    this.Attachments != null &&
                    input.Attachments != null &&
                    this.Attachments.SequenceEqual(input.Attachments)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = 41;
                if (this.Id != null)
                    hashCode = hashCode * 59 + this.Id.GetHashCode();
                if (this.IsDeleted != null)
                    hashCode = hashCode * 59 + this.IsDeleted.GetHashCode();
                if (this.CreatedBy != null)
                    hashCode = hashCode * 59 + this.CreatedBy.GetHashCode();
                if (this.CreatedOn != null)
                    hashCode = hashCode * 59 + this.CreatedOn.GetHashCode();
                if (this.DeletedBy != null)
                    hashCode = hashCode * 59 + this.DeletedBy.GetHashCode();
                if (this.DeleteOn != null)
                    hashCode = hashCode * 59 + this.DeleteOn.GetHashCode();
                if (this.Timestamp != null)
                    hashCode = hashCode * 59 + this.Timestamp.GetHashCode();
                if (this.UpdatedOn != null)
                    hashCode = hashCode * 59 + this.UpdatedOn.GetHashCode();
                if (this.UpdatedBy != null)
                    hashCode = hashCode * 59 + this.UpdatedBy.GetHashCode();
                if (this.Name != null)
                    hashCode = hashCode * 59 + this.Name.GetHashCode();
                if (this.State != null)
                    hashCode = hashCode * 59 + this.State.GetHashCode();
                if (this.StateMessage != null)
                    hashCode = hashCode * 59 + this.StateMessage.GetHashCode();
                if (this.IsLocked != null)
                    hashCode = hashCode * 59 + this.IsLocked.GetHashCode();
                if (this.LockedBy != null)
                    hashCode = hashCode * 59 + this.LockedBy.GetHashCode();
                if (this.LockedOnUTC != null)
                    hashCode = hashCode * 59 + this.LockedOnUTC.GetHashCode();
                if (this.LockedUntilUTC != null)
                    hashCode = hashCode * 59 + this.LockedUntilUTC.GetHashCode();
                if (this.LockedEndTimeUTC != null)
                    hashCode = hashCode * 59 + this.LockedEndTimeUTC.GetHashCode();
                if (this.ExpireOnUTC != null)
                    hashCode = hashCode * 59 + this.ExpireOnUTC.GetHashCode();
                if (this.PostponeUntilUTC != null)
                    hashCode = hashCode * 59 + this.PostponeUntilUTC.GetHashCode();
                if (this.ErrorCode != null)
                    hashCode = hashCode * 59 + this.ErrorCode.GetHashCode();
                if (this.ErrorMessage != null)
                    hashCode = hashCode * 59 + this.ErrorMessage.GetHashCode();
                if (this.ErrorSerialized != null)
                    hashCode = hashCode * 59 + this.ErrorSerialized.GetHashCode();
                if (this.Source != null)
                    hashCode = hashCode * 59 + this.Source.GetHashCode();
                if (this.Event != null)
                    hashCode = hashCode * 59 + this.Event.GetHashCode();
                if (this.ResultJSON != null)
                    hashCode = hashCode * 59 + this.ResultJSON.GetHashCode();
                if (this.QueueId != null)
                    hashCode = hashCode * 59 + this.QueueId.GetHashCode();
                if (this.Type != null)
                    hashCode = hashCode * 59 + this.Type.GetHashCode();
                if (this.JsonType != null)
                    hashCode = hashCode * 59 + this.JsonType.GetHashCode();
                if (this.DataJson != null)
                    hashCode = hashCode * 59 + this.DataJson.GetHashCode();
                if (this.LockTransactionKey != null)
                    hashCode = hashCode * 59 + this.LockTransactionKey.GetHashCode();
                if (this.RetryCount != null)
                    hashCode = hashCode * 59 + this.RetryCount.GetHashCode();
                if (this.Priority != null)
                    hashCode = hashCode * 59 + this.Priority.GetHashCode();
                if (this.PayloadSizeInBytes != null)
                    hashCode = hashCode * 59 + this.PayloadSizeInBytes.GetHashCode();
                if (this.Attachments != null)
                    hashCode = hashCode * 59 + this.Attachments.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// To validate all properties of the instance
        /// </summary>
        /// <param name="validationContext">Validation context</param>
        /// <returns>Validation Result</returns>
        IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }
}
