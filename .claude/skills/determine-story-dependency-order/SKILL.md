---
name: determine-story-dependency-order
description: Determine the order of story dependencies for a given component when migrating from one framework to another
arguments: [epics, output]
argument-hints: [epics-folder] [output-filepath]
disable-model-invocation: true
---

# Argument Validation
- Verify that the provided filepath ${epics} exists and contains valid story files. 
- Ensure that the ${output} path is writable.
- Ensure that the ${output} file name ends with '.json'.
If any of the above validations fail, return an appropriate error message and halt execution.


# Gather Epics
Load *all* the epic files from the specified folder.
Identify the dependency order of the epics based on their relationships and dependencies.
For each epic determine the order of the stories within it based on their dependencies.


# Output the ordered list of stories as json
Each story should be represented as an object with the following structure:
```json
{
  "implementation_order": 1,
  "story-id": "story-01-01",
  "completed": false
}
```

Write the ordered list of stories to the specified output file in JSON format. 
- Implementation order, 1 is first, 2 is second, etc.
- The story-id value should be the story.id from the epic files.
- All completed values should be set to false.
- Ensure that the output is properly formatted json.

Output the json to ${output} and confirm that the file has been written successfully.
