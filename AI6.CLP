(deffacts the-initial-state
(state 1 0 1))

(deffacts opposites
  (opposite-of 1 0)
  (opposite-of 0 1))

(defrule first-second
(state ?z1 ?z2 ?z3)
(opposite-of ?z1 ?zz1)
(opposite-of ?z2 ?zz2)
(not (exists (state ?zz1 ?zz2 ?z3)))
=>
(assert (state ?zz1 ?zz2 ?z3))
(assert (link ?z1 ?z2 ?z3 to ?zz1 ?zz2 ?z3)))

(defrule second-third
(state ?z1 ?z2 ?z3)
(opposite-of ?z2 ?zz2)
(opposite-of ?z3 ?zz3)
(not (exists (state ?z1 ?zz2 ?zz3)))
=>
(assert (state ?z1 ?zz2 ?zz3))
(assert (link ?z1 ?z2 ?z3 to ?z1 ?zz2 ?zz3)))

(defrule first-third
(state ?z1 ?z2 ?z3)
(opposite-of ?z1 ?zz1)
(opposite-of ?z3 ?zz3)
(not (exists (state ?zz1 ?z2 ?zz3)))
=>
(assert (state ?zz1 ?z2 ?zz3))
(assert (link ?z1 ?z2 ?z3 to ?zz1 ?z2 ?zz3)))


(defrule reverse-path
(link ?x1 ?x2 ?x3 to ?xx1 ?xx2 ?xx3)
=>
(assert (path ?x1 ?x2 ?x3 to ?xx1 ?xx2 ?xx3
(str-cat "(" ?x1 ?x2 ?x3 ") -->
(" ?xx1 ?xx2 ?xx3 ")"))))

(defrule find-path
(path ?x1 ?x2 ?x3 to ?xx1 ?xx2 ?xx3 ?route)
(link ?xx1 ?xx2 ?xx3 to ?xxx1 ?xxx2 ?xxx3)
=>
(assert (path ?x1 ?x2 ?x3 to ?xxx1 ?xxx2 ?xxx3
(str-cat ?route " --> (" ?xxx1 ?xxx2 ?xxx3 ")"))))

(defrule print-solutions (path 1 0 1 to 0 0 0 ?route) =>
(printout t ?route crlf))
