# LinkerSharp

LinkerSharp is (nowadays) a pure experimental attempt to port the most basic features of the Apache Camel Framework to a C#.NET flavor:

- A specialized DSL
- Message routing with transactional behaviour
- An easy way of adding custom endpoint components.

The original Java project can be found here: https://github.com/apache/camel

## Goals

### Components
- [x] File Endpoint component (**FILEConsumer** and **FILEProducer**)
- [x] FTP Endpoint component (**FTPConsumer** and **FTPProducer**)
- [x] Direct Endpoint component (**DIRECTConsumer** and **DIRECTProducer**)
- [ ] SQL Endpoint component (**SQLConsumer** and **SQLProducer**)

### Routing
- [x] **RouteBuilder** with***From(string Uri)*** method.
- [x] **RouteDefinition** with the following methods:
	- [x] ***From(string Uri)***
	- [x] ***SetBody(string Content, [bool Append])***
	- [x] ***SetHeader(string Key, string Value)***
	- [x] ***Process(Processor Processor)***
	- [ ] ***Split(Splitter Splitter)***
	- [x] ***Enrich(string Uri)***
	- [x] ***To(string Uri)***
- [ ] Routes' real transactional behaviour
- [ ] Parallel processing

### Data
- [x] Message encapsulation into a transaction model (which is only 'transactional' in name)

### Unit Testing
- [ ] Routes Testing Framework

Let's carry on the good work! :metal:
