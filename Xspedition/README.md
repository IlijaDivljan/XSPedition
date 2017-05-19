# XSPedition

Purpose of this document is to describe Corporate Action Timeline prototype developed by XSPedition team during the InvovateIN48 2017.
Document contains following sections:
  - Technologies and architecture
  - Purpose of prototype
  - Proprietary software and Open source projects
  - Usage
  - Why it's cool?!

### Technologies and architecture

XSPedition Corporate Action Timeline is based on client-server architecture, supporting real-time communication between the two. Server is decouple into two parts, where first part is responsible for the real-time update of the client's data, and the second part exposes HTTP services that process and transforms input into analytical data.

That data is then served to the client for further analysis.

### Purpose of prototype

XSPedition Corporate Action Tracker and Timeline is designed as a quick and intuitive tool for analysis of processes involved in Corporate Action lifecycle, as well as the individual Corporate Action Events.

### Proprietary software and Open source projects

* [ASP.NET MVC] - Powerful, patterns-based way to build dynamic websites!
* [ASP.NET SignalR] - Makes developing real-time web functionality easy.
* [ASP.NET Web API 2] - Framework to build HTTP services that reach a broad range of clients, including browsers and mobile devices. ASP.
* [Microsoft Azure] - Open, flexible, enterprise-grade cloud computing platform.
* [Microsoft Azure SQL Database] - Developer's cloud database service.
* [knockout.js] - Standalone JavaScript implementation of the Model-View-ViewModel pattern with templates.
* [highcharts.js] - World's most popular JavaScript charting engine.
* [vis.js] - A dynamic, browser based visualization library.
* [jQuery] - Cross-platform JavaScript library.
* [jQuery-UI] - UI cross-platform JavaScript library.

### Usage

Prototype is open to the outside world via HTTP endpoints which listen for the incoming data, formatted as JSON. System accepts, transforms and analyses input data, and executes high-performance analytics in regards to Corporate Action events in real-time.


### Why it's cool?!

With its effective visual presentation it is an easy-to-use tool which briefly provides vital insight on state of health of Corporate Action Processing.

[//]: # (These are reference links used in the body of this note and get stripped out when the markdown processor does its job. There is no need to format nicely because it shouldn't be seen. Thanks SO - http://stackoverflow.com/questions/4823468/store-comments-in-markdown-syntax)

   [ASP.NET MVC]: <https://www.asp.net/mvc>
   [ASP.NET SignalR]: <http://signalr.net/>
   [ASP.NET Web API 2]: <https://www.asp.net/web-api>
   [Microsoft Azure]: <https://azure.microsoft.com>
   [Microsoft Azure SQL Database]: <https://docs.microsoft.com/en-us/azure/sql-database>
   [knockout.js]: <http://knockoutjs.com>
   [highcharts.js]: <https://www.highcharts.com/>
   [vis.js]: <http://visjs.org/>
   [jQuery]: <http://jquery.com>
   [jQuery-UI]: <http://jqueryui.com>
