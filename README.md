# LinkerSharp

LinkerSharp is (nowadays) a pure experimental attempt to port the most basic features of the Apache Camel Framework to a C#.NET flavor:

- A specialized DSL
- Message routing with transactional behaviour
- An easy way of adding custom endpoint components.

The original Java project can be found here: https://github.com/apache/camel

## Achieved for now
- Working File Endpoint component (**FILEConsumer** and **FILEProducer**)
- Message encapsulation into a transaction model (which is only 'transactional' in name)
- **RouteBuilder** with***From(string Uri)*** method.
- **RouteDefinition** with the following methods:
	- ***From(string Uri)***
	- ***SetBody(string Content, [bool Append])***
	- ***SetHeader(string Key, string Value)***
	- ***Process(Processor Processor)***
	- ***Enrich(string Uri)***
	- ***To(string Uri)***

## Next Goals
- Create a working FTP component (**FTPConsumer** and **FTPProducer**)
- Routes' real transactional behaviour
