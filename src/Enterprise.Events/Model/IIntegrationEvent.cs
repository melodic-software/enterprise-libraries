namespace Enterprise.Events.Model;
// NOT sure if these will be needed for serialization purposes or not
// It'd be great to externalize these or use a partial definition

//[JsonDerivedType(typeof(RoomAddedIntegrationEvent), typeDiscriminator: nameof(RoomAddedIntegrationEvent))]
//[JsonDerivedType(typeof(RoomRemovedIntegrationEvent), typeDiscriminator: nameof(RoomRemovedIntegrationEvent))]
//[JsonDerivedType(typeof(SessionScheduledIntegrationEvent), typeDiscriminator: nameof(SessionScheduledIntegrationEvent))]
public partial interface IIntegrationEvent : IEvent;