




/*


so the thing is if u want to past var in http request u have to use variables

with C# dont forget to turn c# heres an exemple for URL
i used similiar one but with the variable /currentvalue/ from for each loop

heres exemple 

return new {
    id = Variables.CurrentValue.id,
    quantity = Variables.CurrentValue.quantity
};
 this one is obviosly for the content json body

and this one for URL dont forget to turn C# on

return $"https://reqres.in/api/users/{Variables.UserId}";


ALSO IMPORTANT DONT FORGET TO CHANGE THE DESESION ACTIVITY
CUZ IT CHECK ISLOW VAR WHICH ASSOCIATED WITH FIRSTSTOCK NOT STOCK

when ever i find something need to remember i update this file 
also variables should be set in the context so they can be retrieved later
so basicly u will USE ELSA With Custom ACTIVITIES most of the time 



i will need to find a way with the notification probably related to 
TOKENS and and roles and stuff that could be complicated
update the notification dosent really have something to do with roles
just can get access from DB like any other Data gettin a workflow
specifik to user account gonna be hard

*/

