﻿namespace Enterprise.Messages.Core.Model;

/// <summary>
/// This is a generic marker interface for messaging within the system.
/// This interface can be implemented by any class that represents a message,
/// facilitating standardized handling, routing, and processing across diverse systems.
/// It is designed to be flexible and applicable to a variety of messaging contexts,
/// and is not limited to commands, queries, or events.
/// </summary>
public interface IMessage;