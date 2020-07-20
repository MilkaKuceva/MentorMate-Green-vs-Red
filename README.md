# MentorMate-Red-vs-Green

MentorMate-Red-vs-Green is a .NET Core 3.1 ConsoleApp.

## Description

![Description taken from the image in the official site](https://lh6.googleusercontent.com/aQF3JLAC8AmBhRtTEha1q6U4fi60jWJ_oH-RcpX6sAgWW4uPw_LPro-QmjYX9jG9-lOro6ZJ_MkNHoZDXbhIPcoQbWlEPliRVhNmgUTibTPL8U2IV6Ocw62M4hjE=w693)

## Summary

The parameters are passed to the .exe in the args[]. The state of the grid is stored in a 2-dimentional array. The grid uses another array for the computation of the next generation.
The goal is to count the green neighbours only. To avoid extra if's, there are different methods (named "Do%") for each "special" case where a cell doesn't have 8 neighbours.

## License
[MIT](https://choosealicense.com/licenses/mit/)
