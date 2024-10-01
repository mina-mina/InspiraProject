# InpiraProject

#Technical Assumptions for quick solution delivery of this challenge:

-User or API request authentication are not implemented for time saving 
-Form and Submission Catalogue data is submitted before request received by this service
-Form and Submission catalogues must already have data before request to this service
-a userId and a submissionId are already saved in the database before request is made to this service.
-a userId, submissionId and a correlationId are provided in the HttpContext Items
-SSNInternalCheck SOAP request/response is to be mocked using internal service
-No separate serivces to validate response/request handling like it's done in Groovy script
-A middleware to be used to provide HttpContext validation of the request
-we assume the same groovy script HTTP response format for all statuscodes
-Provided Unit and integration tests are to be used to demonstrate this service outputs
-Test cases are to demonstrate various service usage invoke by the User/client application request
- for quick delivery, Logging and code Exception handlings are done in simple form and at minimum


Before running the tests:

-Setup MongoDB 
	- Create Db as "Avoka"
	- Create below catalogues:
			-Submission
			-SubmissionProperty - import SeedSubmissionProperties.json file to seed data
			-Form
	-setup Mongodb Configs in Tests project appsettings
	-sample config:

		"AvokaDatabase": {
			"ConnectionString": "mongodb://localhost:27017",
			"DatabaseName": "Avoka"
		  }
	-Run tests and check tests results

In place of some above the assumptions I have put down some TODO comments in the code.

Please contact me for any questions or concerns.

Ehsan :)