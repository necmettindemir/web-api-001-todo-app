

Properties

1) Asp.NET core 2.1 API has been developed

2) Dapper (light ORM library) 1.60.6 has been used between database and API

3) Swagger has been used for documentation

4) DI has been performed. Working with MSSQL|MYSQL|ORACLE|etc is possible..
   
   To perfoms parametrized DI on non-API apps install from nuget :  Microsoft.Extensions.DependencyInjection

   Register this service in startup

   exp :  services.AddTransient<IAuthRepo>(s => new AuthRepoMSSQL(ConnectionString, SecretKey)); 

5) AutoMapper has been used in many Model-2-DTO conversions

6) JWT has been developed for authentication

7) xUnit has been used for testing

8) ReactJS for web, React Native for mobile have been developed to use this API.

--------

login test 

{
"Email":"deneme@deneme.com",
"Password":"123",
"LanguageCode":"EN"
}

----

register test

{
 "Email":"deneme@deneme.com",
 "Password":"123",        
 "FirstName":"Ahmet",
 "LastName":"Çelik",
 "City":"Çankırı-elazığ",
 "CountryId":"1"
}

-----



