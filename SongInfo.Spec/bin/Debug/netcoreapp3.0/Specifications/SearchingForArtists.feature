Feature: Searching For Artists

Scenario: Search For Artists Who Do Not Exist
    Given an artist search prompt
    When 'some crazy artist name which would never exist' is entered
    Then the message 'Found no artists' is returned

Scenario: Search For An Artist With A Single Match
    Given an artist search prompt
    When 'Arctic Monkeys' is entered
    Then the message 'Name: Arctic Monkeys' is returned
    And the message 'No of works: 25' is returned
    And the message 'Total number of words in all works: 4365' is returned

Scenario: Search For An Artist With Multiple Matches
    Given an artist search prompt
    When 'Arctic' is entered
    Then the message 'Found 54 artists' is returned