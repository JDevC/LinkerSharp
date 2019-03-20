# LinkerSharp

LinkerSharp is (nowadays) a pure experimental attempt to port the most basic features of the Apache Camel Framework to a C#.NET flavor:

- A specialized DSL
- Message routing with transactional behaviour
- An easy way for adding custom endpoint components.

The original Java project can be found here: https://github.com/apache/camel

## Goals

### Components
- [x] File Endpoint component (**FILEConsumer** and **FILEProducer**)
- [x] FTP Endpoint component (**FTPConsumer** and **FTPProducer**)
- [x] Direct Endpoint component (**DIRECTConsumer** and **DIRECTProducer**)
- [ ] Email Endpoint component (**EMAILConsumer** and **EMAILProducer**)
- [ ] Timer Endpoint component (**TIMERConsumer**)
- [ ] SQL Endpoint component (**SQLConsumer** and **SQLProducer**)

### Behaviour
- [x] Isolated processes by context
- [ ] Unordered route's injection
- [ ] Parallel processing

### Routing
- [x] **RouteBuilder** with***From(string Uri)*** method.
- [x] **RouteDefinition** with the following methods:
	- [x] ***From(string Uri)***
	- [x] ***SetBody(string Content, [bool Append])***
	- [x] ***SetHeader(string Key, string Value)***
	- [ ] ***Format(Formatter Formatter)***
	- [x] ***Process(Processor Processor)***
	- [x] ***Enrich(string Uri)***
	- [x] ***To(string Uri)***
- [ ] Routes' real transactional behaviour

### Data
- [x] Message encapsulation into a transaction model (which is only 'transactional' in name)
- [ ] Common transaction headers:
	- [ ] Delay

### Unit Testing
- [ ] Routes Testing Framework

Let's carry on the good work! :metal:
