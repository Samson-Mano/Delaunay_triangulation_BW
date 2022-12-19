# Delaunay triangulation BW

### Bowyer Watson Algorithm

This is a C# implementation of Bowyer Watson Delaunay triangulation. C# really dont want us to use pointers. This repository is my attempt of safe code Delaunay triangulation without using pointers.

### Why Bowyer watson algorithm?

Bowyer watson is the better algorithm for constrained delunay triangulation. This code is my take of Bowyer watson algorithm. Fundametnally its the same but implementaion wise different.

### What about performance?

In my PC, 1000pts -> 141 ms, 2000pts ->	511ms, 3000pts ->	1085ms, 5000pts -> 2918ms, 10000pts ->	11679ms. Not great but I think better performance can be achieved using Divide and conquer approach.

### Any bugs?

Didnot encounter any bugs. If you find anything raise an issue. Coincident points will give error message.

![BW_delaunay_results](/Delaunay_triangulation_BW/images/bw_results.png)
