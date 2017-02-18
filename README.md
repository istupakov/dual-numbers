# Dual Numbers
Calculator on C# with auto differentiation on dual numbers.
For compilation need .NET Core SDK (RC4) or VS2017.
Use Roslyn C# Scripts for expression parsing.

Some examples:
```
(prompt)> Sqrt(2*2)
2

(prompt)> Vec(1, 2, 3)/2
[0.5, 1, 1.5]

(prompt)> t => t*t
Enter expr or 'q': 3
f(t) = 9, f'(t) = 6, f"(t) = 2

(prompt)> t => Vec(Sin(t), Cos(t), 0)
Enter expr or 'q': 0
f(t) = [0, 1, 0], f'(t) = [1, 0, 0], f"(t) = [0, -1, 0]

(prompt)> p => p
Enter expr or 'q': Vec(1, 2, 3)
f(p) = [1, 2, 3], div f(p) = 3, curl f(p) = [0, 0, 0]

(prompt)> p => p.Norm
Enter expr or 'q': Vec(1, 2, 3)
f(p) = 3.74165738677394, grad f(p) = [0.267261241912424, 0.534522483824849, 0.801783725737273]
```
