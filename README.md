CHAPS
Correspondence Handling and Processing System

The Ministerial Correspondence Unit (MCU) receives correspondence from MP on behalf of constituents as well as direct from members of the public.
They allocate these to action officers for a response to be drafted and then it is passed to the ministers' office for approval before it is signed by a minister. It is an MVC3.NET solution hosted on SSG servers and is only accessible within the DOM1 domain.

#chapsdotnet
This repo is the upgrade of the previous Chaps - which was written in MVC 3.5 using c# and .net Framework 4.8

Chaps.net uses .net 6, and is also using the MVC design pattern.

#Approach
The upgrade is using the "Strangler Fig Pattern", but will encompase the entire functionality once complete.

#Development Requirements

Use the latest version of Visual Studio for the mac.

#Secrets
To run the application semi-locally (note that we are using a non-local database, connecting to a SQL Server in RDS) you need to set up the following secrets on your dev machine

DB_NAME
RDS_HOSTNAME
RDS_PASSWORD
RDS_PORT
RDS_USERNAME
ClientId
TenantId
Instance
Domain
CallbackPath

Use the command:
$ dotnet user-secrets set "key_name" 'value'

The above secret key-value pairs are stored in LastPass
