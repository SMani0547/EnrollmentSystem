using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace USPSystem.Models
{
    public static class RequirementTypeExtensions
    {
        public static string GetDisplayName(this RequirementType requirementType)
        {
            var memberInfo = requirementType.GetType().GetMember(requirementType.ToString()).FirstOrDefault();
            if (memberInfo != null)
            {
                var displayAttribute = memberInfo.GetCustomAttribute<DisplayAttribute>();
                if (displayAttribute != null)
                {
                    return displayAttribute.Name ?? requirementType.ToString();
                }
            }
            return requirementType.ToString();
        }
    }
} 

