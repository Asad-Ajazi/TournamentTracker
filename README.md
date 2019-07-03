# TournamentTracker

### A single elimination tournament tracker written in c#

This application is designed to be used for both team based and individual style tournaments.

The owner of a tournament is able to create the tournament and assign and entry fee. Prizes are then created which will go to the first, second and third placed teams. This could be a cash prize that is a percentage of the total sum collected from the entry fee, or any other sum of money or item. 

Users are able to create a team and add any number of players, these team will then randomly be assigned a matchup in their bracket and the loser of the matchup is immediately eliminated from the tournament. If there is an odd number of teams, a randomly selected team will automatically move on to the next round. After the final match has been completed a winner is declared and the prizes are distributed.

Application is currently implemented in WinForms but will eventually be transfered to other UI's such as WPF or MVC using created class library that holds the majority of the logic. 

## What I Learnt
- Turning requirements into design documentation to begin planning.
- Using the Dapper framework to execute T-SQL stored procedures in a SQL-Server database.
- Development with WinForms UI to create a fully working application.
- Debugging and Manually testing code to ensure robustness (Future project will likely use unit testing).
- How to write complex logic to meet business requirement.
- The importance of writing clean code.
