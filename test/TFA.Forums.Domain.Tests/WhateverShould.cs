namespace TFA.Forums.Domain.Tests;

public class WhateverShould
{
    public double CalculateDesirability(Interviewer interviewer, DateOnly interviewDate)
    {
        var daysFromLastInterview = interviewDate.DayNumber - interviewer.LastConductedInterviewAt.DayNumber;
        return daysFromLastInterview / interviewer.DesiredIntervalBetweenInterviews.TotalDays;
    }

    public void Do(IReadOnlyCollection<Interviewer> interviewers, DateOnly interviewDate)
    {
        // extract the interviewers

        var bestMatchingInterviewers = interviewers
            .OrderByDescending(interviewer => CalculateDesirability(interviewer, interviewDate))
            .Take(2)
            .ToArray();

        if (bestMatchingInterviewers is not [var interviewer1, var interviewer2])
        {
            throw new Exception();
        }

        // store the interview
    }
}

public record Interviewer(string Name, DateOnly LastConductedInterviewAt, TimeSpan DesiredIntervalBetweenInterviews);