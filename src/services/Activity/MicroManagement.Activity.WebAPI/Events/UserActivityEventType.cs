using System.Text.Json.Serialization;

namespace MicroManagement.Activity.WebAPI.Events;


[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UserActivityEventType
{
    TimeSessionStarted,
    TimeSessionStopped,
}