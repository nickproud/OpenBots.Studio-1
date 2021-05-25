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
    /// AllEmailAttachmentsViewModelPaginatedList
    /// </summary>
    [DataContract]
        public partial class AllEmailAttachmentsViewModelPaginatedList :  IEquatable<AllEmailAttachmentsViewModelPaginatedList>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AllEmailAttachmentsViewModelPaginatedList" /> class.
        /// </summary>
        /// <param name="pageNumber">pageNumber.</param>
        /// <param name="pageSize">pageSize.</param>
        /// <param name="totalCount">totalCount.</param>
        /// <param name="items">items.</param>
        /// <param name="completed">completed.</param>
        /// <param name="started">started.</param>
        /// <param name="impediments">impediments.</param>
        /// <param name="parentId">parentId.</param>
        public AllEmailAttachmentsViewModelPaginatedList(int? pageNumber = default(int?), int? pageSize = default(int?), int? totalCount = default(int?), List<AllEmailAttachmentsViewModel> items = default(List<AllEmailAttachmentsViewModel>), int? completed = default(int?), int? started = default(int?), int? impediments = default(int?), Guid? parentId = default(Guid?))
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.TotalCount = totalCount;
            this.Items = items;
            this.Completed = completed;
            this.Started = started;
            this.Impediments = impediments;
            this.ParentId = parentId;
        }
        
        /// <summary>
        /// Gets or Sets PageNumber
        /// </summary>
        [DataMember(Name="pageNumber", EmitDefaultValue=false)]
        public int? PageNumber { get; set; }

        /// <summary>
        /// Gets or Sets PageSize
        /// </summary>
        [DataMember(Name="pageSize", EmitDefaultValue=false)]
        public int? PageSize { get; set; }

        /// <summary>
        /// Gets or Sets TotalCount
        /// </summary>
        [DataMember(Name="totalCount", EmitDefaultValue=false)]
        public int? TotalCount { get; set; }

        /// <summary>
        /// Gets or Sets Items
        /// </summary>
        [DataMember(Name="items", EmitDefaultValue=false)]
        public List<AllEmailAttachmentsViewModel> Items { get; set; }

        /// <summary>
        /// Gets or Sets Completed
        /// </summary>
        [DataMember(Name="completed", EmitDefaultValue=false)]
        public int? Completed { get; set; }

        /// <summary>
        /// Gets or Sets Started
        /// </summary>
        [DataMember(Name="started", EmitDefaultValue=false)]
        public int? Started { get; set; }

        /// <summary>
        /// Gets or Sets Impediments
        /// </summary>
        [DataMember(Name="impediments", EmitDefaultValue=false)]
        public int? Impediments { get; set; }

        /// <summary>
        /// Gets or Sets ParentId
        /// </summary>
        [DataMember(Name="parentId", EmitDefaultValue=false)]
        public Guid? ParentId { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class AllEmailAttachmentsViewModelPaginatedList {\n");
            sb.Append("  PageNumber: ").Append(PageNumber).Append("\n");
            sb.Append("  PageSize: ").Append(PageSize).Append("\n");
            sb.Append("  TotalCount: ").Append(TotalCount).Append("\n");
            sb.Append("  Items: ").Append(Items).Append("\n");
            sb.Append("  Completed: ").Append(Completed).Append("\n");
            sb.Append("  Started: ").Append(Started).Append("\n");
            sb.Append("  Impediments: ").Append(Impediments).Append("\n");
            sb.Append("  ParentId: ").Append(ParentId).Append("\n");
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
            return this.Equals(input as AllEmailAttachmentsViewModelPaginatedList);
        }

        /// <summary>
        /// Returns true if AllEmailAttachmentsViewModelPaginatedList instances are equal
        /// </summary>
        /// <param name="input">Instance of AllEmailAttachmentsViewModelPaginatedList to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(AllEmailAttachmentsViewModelPaginatedList input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.PageNumber == input.PageNumber ||
                    (this.PageNumber != null &&
                    this.PageNumber.Equals(input.PageNumber))
                ) && 
                (
                    this.PageSize == input.PageSize ||
                    (this.PageSize != null &&
                    this.PageSize.Equals(input.PageSize))
                ) && 
                (
                    this.TotalCount == input.TotalCount ||
                    (this.TotalCount != null &&
                    this.TotalCount.Equals(input.TotalCount))
                ) && 
                (
                    this.Items == input.Items ||
                    this.Items != null &&
                    input.Items != null &&
                    this.Items.SequenceEqual(input.Items)
                ) && 
                (
                    this.Completed == input.Completed ||
                    (this.Completed != null &&
                    this.Completed.Equals(input.Completed))
                ) && 
                (
                    this.Started == input.Started ||
                    (this.Started != null &&
                    this.Started.Equals(input.Started))
                ) && 
                (
                    this.Impediments == input.Impediments ||
                    (this.Impediments != null &&
                    this.Impediments.Equals(input.Impediments))
                ) && 
                (
                    this.ParentId == input.ParentId ||
                    (this.ParentId != null &&
                    this.ParentId.Equals(input.ParentId))
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
                if (this.PageNumber != null)
                    hashCode = hashCode * 59 + this.PageNumber.GetHashCode();
                if (this.PageSize != null)
                    hashCode = hashCode * 59 + this.PageSize.GetHashCode();
                if (this.TotalCount != null)
                    hashCode = hashCode * 59 + this.TotalCount.GetHashCode();
                if (this.Items != null)
                    hashCode = hashCode * 59 + this.Items.GetHashCode();
                if (this.Completed != null)
                    hashCode = hashCode * 59 + this.Completed.GetHashCode();
                if (this.Started != null)
                    hashCode = hashCode * 59 + this.Started.GetHashCode();
                if (this.Impediments != null)
                    hashCode = hashCode * 59 + this.Impediments.GetHashCode();
                if (this.ParentId != null)
                    hashCode = hashCode * 59 + this.ParentId.GetHashCode();
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
