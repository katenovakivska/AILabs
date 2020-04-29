start:- initial_state(State),breadth_first_search([State],[]).

%% ініціалзація початкового стану, де h-герб, t-цифра
	initial_state([h,t,h]).

%%стани,що вважаються розв'язком
%%всі монети лежать догори цифрою
    goal_state([t,t,t]).
%%всі монети лежать догори гербом
    goal_state([h,h,h]).

%%пошук вширину
breadth_first_search([Parent|_],[]):-goal_state(Parent),!.
breadth_first_search([Parent|Queue0],In_Order0) :-member(Parent,Queue0),
    not(member(Parent,In_Order0)),
    goal_state(Parent),
    append(In_Order0,[Parent],In_Order1),
    print_path(In_Order1),!.
breadth_first_search([Parent|Queue0],In_Order0) :-
    append(In_Order0,[Parent],In_Order1),
    findall(Child,move(Parent, Child),Children),
    append(Queue0,Children,Queue),
    breadth_first_search(Queue,In_Order1).

%% перегортання герба дає цифру, перегортання цифри-герб
	opposite(h,t).  
	opposite(t,h).

%%три можливі перевертання монет 
move([A,B,C],[A1,B1,C]):- opposite(A,A1),opposite(B,B1).  % перша та друга
move([A,B,C],[A,B1,C1]):- opposite(C,C1),opposite(B,B1).  % друга та третя 
move([A,B,C],[A1,B,C1]):- opposite(A,A1),opposite(C,C1).  % перша та третя 

%%виведення шляху
print_path(L):- forall(member(X,L),(write(X),nl)).
