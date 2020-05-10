start:- initial_state(Start), rbfs([], [ (Start, 0/0/0) ], 99999, _, yes, Solution),
   reverse(Solution,Solution1), print_path(Solution1).

% ініціалзація початкового стану
initial_state( [initial, [jug1,9,0], [jug2,5,0], [reserve,14,14]]).

% стани,що вважаються розв'язком
% отрмаємо три літри у більшому відрі
goal_state( [_, [jug1,_,3], [jug2,_,_], [reserve,_,_]]).
% отрмаємо три літри у меншому відрі
goal_state( [_, [jug1,_,_], [jug2,_,3], [reserve,_,_]]).

% якщо об'єм води першому відрі менше/рівно, ніж вільного місця у другому, 
% то заповнюємо повністю друге відро
pour( [Jug1, Capacity1, Amount1], [Jug2, Capacity2, Amount2], % initial jug states
      [Jug1 ,Capacity1, 0],   [Jug2, Capacity2, New],
      Cost):- 
       Amount1 =< (Capacity2 - Amount2), 
       Cost is Amount1 + Amount2,
       New is Amount1 + Amount2.

% якщо об'єм води першому відрі більше, ніж вільного місця у другому, 
% то заповнюємо повністю друге відро
pour( [Jug1, Capacity1, Amount1], [Jug2, Capacity2, Amount2], 
      [Jug1 ,Capacity1, New],   [Jug2, Capacity2, Capacity2],
      Cost):- 
       Amount1 > (Capacity2 - Amount2), 
       Cost is Amount1 - (Capacity2 - Amount2),
       New is Amount1 - (Capacity2 - Amount2).

% шість можливих переливань
move( [_, A1,B1,C], [pour_jug1_to_jug2, A2,B2,C], Cost) :- pour(A1,B1,A2,B2,Cost).
move( [_, A1,B,C1], [pour_jug1_to_reserve, A2,B,C2], Cost) :- pour(A1,C1,A2,C2,Cost).
move( [_, A1,B1,C], [pour_jug2_to_jug1, A2,B2,C], Cost) :- pour(B1,A1,B2,A2,Cost).
move( [_, A,B1,C1], [pour_jug2_to_reserve, A,B2,C2], Cost) :- pour(B1,C1,B2,C2,Cost).
move( [_, A1,B,C1], [pour_reserve_to_jug1, A2,B,C2], Cost) :- pour(C1,A1,C2,A2,Cost).
move( [_, A,B1,C1], [pour_reserve_to_jug2, A,B2,C2], Cost) :- pour(C1,B1,C2,B2,Cost).

% вершина Node = (стан, вартість шляху до поточної вершини/значення оцінки f/найкраще значення оцінки f)
% аргументи rbfs(шлях,дочірні вершини,найкраще значення оцінки f-F,індикатор успіху,шлях розв'язку)
rbfs(_, [ (_, _/_/BestF) | _], Bound, BestF, no, _) :-
    BestF > Bound, !.

rbfs(Path, [ (Node, _/F/BestF) | _], _, _, yes, [Node | Path]) :-
    F = BestF,     % значення оцінки f має найкраще значення
    goal_state(Node),!. % досягнута ціль

rbfs(_, [], _, _, never, _) :- !.    % немає вихідних станів

rbfs(Path, [ (Node, G/F/BestF) | Ns], FBound, NewFF, Solved, Sol) :-
    BestF =< FBound,                     % якщо не досягнута межа, шукаємо вихідні значення
    findall(Child/Cost, 
            (move(Node, Child, Cost), \+ member(Child, Path)),
            Children),
    inherit(F, BestF, InheritedBestF),     % дочірні вузли можуть наслідувати найкраще значення оцінки
    succlist(G, InheritedBestF, Children, SuccNodes),    % упорядкування дочірніх вершин
    bestf(Ns, NextBestF),          % знаходження найближчого конкурента по найкращьому значенню
    min(FBound, NextBestF, Bound2), !,
    rbfs([Node | Path], SuccNodes, Bound2, NewBestF2, Solved2, Sol),
    continue(Path, [(Node,G/F/NewBestF2)|Ns], FBound, NewFF, Solved2, Solved, Sol).

continue(Path, [_ | Ns], Bound, NewBestF, never, Solved, Sol) :- !,
    rbfs(Path, Ns, Bound, NewBestF, Solved, Sol). % вузол є кінцем

continue(_, _, _, _, yes, yes, _).

continue(Path, [ N | Ns], Bound, NewBestF, no, Solved, Sol) :-
    insert(N, Ns, NewNs), !,         % перевірка чи вузли упорядковані
    rbfs(Path, NewNs, Bound, NewBestF, Solved, Sol).

succlist(_, _, [], []).

succlist(G0, InheritedBestF, [Node/C | NCs], Nodes) :-
    G is G0 + C,
    h(Node, H),
    F is G + H,
    max(F, InheritedBestF, BestF),
    succlist(G0, InheritedBestF, NCs, Nodes2),
    insert((Node, G/F/BestF), Nodes2, Nodes).

h(Goals,H):-
 start(State),
 delete_all(Goals,State,Unsatisfied),
 length(Unsatisfied,H).

start([clear(3),clear(4),clear(5),clear(6)]).

delete_all( [], _, []).

delete_all( [X | L1], L2, Diff)  :-
  member( X, L2), !,
  delete_all( L1, L2, Diff).

delete_all( [X | L1], L2, [X | Diff])  :-
  delete_all( L1, L2, Diff).

inherit(F, BestF, BestF) :-                % дочірня вершина наслідує найкраще значення оцінки  
    BestF > F, !.                          % якщо воно більше за значення оцінки батьківської вершини

inherit(_, _, 0).



insert((N, G/F/BestF), Nodes, [ (N, G/F/BestF) | Nodes]) :-
    bestf(Nodes, BestF2),
    BestF =< BestF2, !.

insert(N, [N1 | Ns], [N1 | Ns1]) :-
    insert(N, Ns, Ns1).



bestf([ (_, _/_/BestF) | _], BestF).      % для першої вершини - найкраще значення

bestf([], 99999).                         % якщо не має вихідних, то велике число


min(X, Y, X) :-
    X  =<  Y, !.
min(_, Y, Y).


max(X, Y, X) :-
    X  >=  Y, !.
max(_, Y, Y).
%%виведення шляху
print_path(L):- forall(member(X,L),(write(X),nl)).