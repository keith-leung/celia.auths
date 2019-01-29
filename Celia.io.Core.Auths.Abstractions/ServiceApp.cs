using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Celia.io.Core.MicroServices.Utilities;

namespace Celia.io.Core.Auths.Abstractions
{
    [Table("auths_service_apps")]
    public class ServiceApp
    {
        [Key]
        [MaxLength(50)]
        public string AppId { get; set; }

        [MaxLength(50)]
        [Required]
        public string AppSecret { get; set; }

        //
        // Summary:
        //     Gets or sets a flag indicating if the user could be locked out.
        public virtual bool LockoutEnabled { get; set; }
        //
        // Summary:
        //     Gets or sets the number of failed login attempts for the current user.
        public virtual int AccessFailedCount { get; set; }
        //
        // Summary:
        //     Gets or sets the date and time, in UTC, when any user lockout ends.
        //
        // Remarks:
        //     A value in the past means the user is not locked out.
        [NotMapped]
        public virtual DateTimeOffset? LockoutEnd
        {
            get
            {
                if (LockoutEndTimestamp != null)
                {
                    return new DateTimeOffset(DateTimeUtils.ToDateTimeNow(this.LockoutEndTimestamp.Value));
                }

                return null;
            }
            set
            {
                if (value == null)
                {
                    LockoutEndTimestamp = null;
                }
                else
                {
                    this.LockoutEndTimestamp = DateTimeUtils.GetTimestamp(value.Value.DateTime);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public long? LockoutEndTimestamp { get; set; }
    }
}
