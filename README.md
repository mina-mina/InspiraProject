# InpiraProject

## Technical Assumptions for quick solution delivery of this challenge:
<ul>
	<li>API authentication is not implemented due to time constraints</li>
	<li>Form and Submission Catalogue data is submitted before request received by this service</li>
	<li>Form and Submission catalogues must are populated before the request is made to this service</li>
	<li>The userId and submissionId are saved in the database before the request is made to this service.</li>
	<li>The userId, submissionId and a correlationId are provided in the HttpContext Items</li>
	<li>SSNInternalCheck SOAP request/response structures are mocked using an internal service</li>
	<li>Dependency Validator (in groovy) is not implemented in this solution</li>
	<li>A middleware to be used to provide HttpContext validation of the request</li>
	<li>we assume the same groovy script HTTP response format for all statuscodes</li>
	<li>Provided unit and integration tests are to be used to demonstrate service outputs</li>
	<li>Test cases are used to mock consumption of User/client application request</li>
	<li>Logging and Exception handling are done in simple form due to time constraints</li>
</ul>

## Secuity Considerations
*

## Before running the tests:

<ol>	
	<li>Setup MongoDB </li>
		<li> Create Db as "Avoka"</li>
		<li> Create below catalogues:</li>
			<ul>
				<li>Submission</li>
				<li>SubmissionProperty - import SeedSubmissionProperties.json file to seed data</li>
				<li>Form</li>
			</ul>
		<li>Setup Mongodb Configs in Tests project appsettings</li>
		<li>Sample config:</li>
			```
			"AvokaDatabase": {
				"ConnectionString": "mongodb://localhost:27017",
				"DatabaseName": "Avoka"
			}
			```
	<li>Run tests and check tests results</li>
</ol>

In place of some above the assumptions I have put down some TODO comments in the code.

Please contact me for any questions or concerns.

Ehsan :)