# Chain of Responsibility

## Description
The Chain of Responsibility is a behavioral design pattern that aims to decouple the sender of a request from its receivers 
by allowing multiple objects a chance to handle the request. 
It achieves this by passing the request along a chain of potential handlers until one of them handles it.

## Applicability
- **Multiple Handlers**: Use when more than one object may handle a request, and the handler is not known in advance. 
- The handler should be determined automatically.
- **Dynamic Handler Assignment**: Use when you want to issue a request to one of several objects without specifying the receiver explicitly.
- **Flexible Handling**: Use when the set of objects that can handle a request should be specified or modified dynamically.

## Terminology

### Sender (Client)
The sender, also known as the "client", initiates the request and sends it to the first handler in the chain. 
The sender does not need to know the details of the chain's structure or the identity of the eventual handler.

### Handler
- **Abstract Handler**: Defines an interface for handling requests and, optionally, includes the successor link. 
- This component is responsible for standardizing the handling process and successor linkage.
- **Concrete Handler**: Each concrete handler implements the handling mechanism for specific requests it is responsible for.
- It can access its successor and will handle the request if capable; otherwise, it forwards the request to its successor.

### Successor
The successor is the next handler in the chain which a handler refers to if it cannot handle the request. 
The succession mechanism ensures the collaborative pass-through behavior typical in this pattern.

### Receiver
The receiver is the object in the chain that eventually handles the request. 
The chain's structure ensures that the sender is unaware of the specific receiver, promoting loose coupling in the system.

## Benefits
- **Reduced Coupling**: The sender of the request and its receivers are decoupled, simplifying interactions within the system.
- **Increased Flexibility**: The pattern allows dynamic addition or removal of handlers or alteration of the chain's order, 
- increasing the flexibility of handling logic.
- **Distributed Handling**: Responsibilities for handling requests are distributed among multiple objects, rather than being concentrated in a single handler.

## Disadvantages
- **No Guarantee of Handling**: There is a risk that the request may not be handled at all if no object in the chain considers itself responsible.
- **Debugging Complexity**: The dynamic and distributed nature of the chain can make it challenging to debug and trace the path of request handling.

## Common Usages
- **Event Handling in UI Frameworks**: Managing events like mouse clicks and keyboard inputs in a decoupled manner.
- **Processing Pipelines**: Used in various processing stages of requests, such as in middleware or data processing applications.
- **Conditional Handling**: Useful as a more structured alternative to a series of if/else statements for handling conditional logic.

## Summary
The Chain of Responsibility pattern facilitates the creation of a chain of objects to process requests sequentially. 
Each object examines the request and either handles it or forwards it to the next object in the chain, promoting modularity and flexibility in request processing.