# Digital Apprenticeship Service Reference Data API (BETA)

This solution represents the Reference Data API (currently pre private BETA) code base.

### Build
![Build Status](https://sfa-gov-uk.visualstudio.com/_apis/public/build/definitions/c39e0c0b-7aff-4606-b160-3566f3bbce23/101/badge)


## Running locally

### Requirements

In order to run this solution locally you will need the following installed:

* [SQL Server LocalDB](https://www.microsoft.com/en-us/download/details.aspx?id=52679) - you only need the LocalDB component of SQL Server Express
* [Azure SDK 2.9 (for Visual Studio)](https://azure.microsoft.com/en-us/downloads/) - choose the SDK version appropriate to your version of Visual Studio
* [Azure Storage Explorer](http://storageexplorer.com/)

You should run Visual Studio as an Administrator.

### Setup

* Running the RunBuild.bat will expose any dependency issues
* Running the Database project (SFA.DAS.ReferenceData.Database) will provision the local DB instance with relevent seed data.
* In the Azure Storage Explorer, connect to local storage. In the "(Development)" storage account, create a new local table named "Configuration" and load the following row data:
  * PartitionKey => LOCAL
  * RowKey => SFA.DAS.ReferenceDataAPI_1.0
  * Data => Contents of the environment configuration JSON document (found in src/SFA.DAS.ReferenceData.Api\App_Data\LOCAL_SFA.DAS.ReferenceData.Api_1.0.json)
* Update the connection string in the JSON to point at your LocalDB instance (look around in Visual Studio's SQL Server Object Explorer)


### Running

Running the cloud service will start the web application in your browser.


### Public Sector Organisations Lookup

The API provides a list of all public sector organisation names that is manually retrieved from the Office of National Statistics (ONS) website and stored as json into Azure cloud storage.  

The ONS Data set of public sector organisations can be found here [Public sector organisation (ONS)](https://www.ons.gov.uk/economy/nationalaccounts/uksectoraccounts/datasets/publicsectorclassificationguide/)


To add organisation names simple open up the json file:

**SFA.DAS.SFA.DAS.ReferenceData.Api/App_Data/LOCAL_SFA.DAS.ReferenceData.PublicOrganisationNames_1.0.json**

And add any number organisation names to the list.

You can then store the json text in the same way as the Reference Data configuration (as mentioned above in the Setup section) with the RowKey value of 'SFA.DAS.ReferenceData.PublicOrganisationNames_1.0'

Below is an example of what the file may look like

```
{
	"OrganisationNames": [
		"Organisation 1", 
		"Organisation 2", 
		"Organisation 3"
		]
}
```
