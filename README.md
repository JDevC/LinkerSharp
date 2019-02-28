# LinkerSharp

LinkerSharp is (nowadays) a pure experimental attempt to port the most basic features of the Apache Camel Framework to a C#.NET flavor:

* A specialized DSL
* Message routing with transactional behaviour
* An easy way of adding custom endpoint components.

The original Java project can be found here: https://github.com/apache/camel

## Achieved for now
* Working File Endpoint component (**FILEConsumer** and **FILEProducer**)
* Message encapsulation into a transaction model (which is only 'transactional' in name)
* **RouteBuilder** and **RouteDefinition** with From(), Process() and To() methods

## Next Goals
* Create a working FTP component (**FTPConsumer** and **FTPProducer**)
* Implement the Content Enricher EIP in RouteDefinition.
* Routes' real transactional behaviour
