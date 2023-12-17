using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Web2Prosjektoppgave.api.Utilities;

public class ApiHelperFunctions
{
    // Form names need to be the same as the names in the entity object 
    public static bool UpdatePropertiesDiffer<T1, T2>(T1 existingObject, T2 updateObject)
    {
        var existingObjectProperties = typeof(T1).GetProperties();
        var updateObjectProperties = typeof(T2).GetProperties();

        foreach (var existingProperty in existingObjectProperties)
        {
            var propertyInUpdateObject = updateObjectProperties.FirstOrDefault(p => p.Name == existingProperty.Name);

            if (propertyInUpdateObject != null)
            {
                var existingValue = existingProperty.GetValue(existingObject);
                var updateValue = existingProperty.GetValue(updateObject);

                if (!Equals(existingValue, updateValue))
                {
                    return true;
                }
            }
        }

        return false;
    }
}