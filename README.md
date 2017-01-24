# DiffAPI
A sample project to compare two strings.

- This API is used to compare two base64 encoded binary data.
- In order to compare both data, you need to submit both data in advance
with an arbitrary ID (number).

STEP.

HOW TO RUN.

1. Open the solution file with VS 2015 Community Edition.
2. Restore the NuGet packages.
3. Click Run.

API USAGE

1. PUT /v1/diff/{id}/left
{
    "data" : "the encoded binary data"
}

2. PUT /v1/diff/{id}/right
{
    "data" : "the encoded binary data"
}

2. GET /v1/diff/{id}
{
    diffResultType: "result type" // Could be Equals, SizeDoNotMatch, ContentDoNotMatch
    diffs: [{
        offset: int,
        length: int
    }]
}
