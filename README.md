These Appear In The Start Of The Script Itself Too.

/* NOTES:

   HOW TO USE: To Change Lightmap You Need To Set 'LightmapIndex' To Whatever The Index Of The Lightmap.
   As Soon As You Set It, The Lightmap Will Update (Unless You Enable: 'updateEveryFrame').
   This Obviously Saves A Lot Of Performance, As The Lightmap Switching Does Not Change All The Lightmaps
   In The Scene Every Single Frame.

   In The Script, Add A New 'PseudoLightmapInfo' Into
   The 'PseudoDynamicLightmaps' List. And Set The Directional And Colored Lightmaps Manually OR
   Just Write The Directory Down In The 'PseudoLightmapInfo' And Enable 'GetTexturesFromDirectory'
   To Have The Script Already Set It On Start().

   STN: To Get Lightmap Info Through Directory You Need All The Name Of The Directional Lightmaps To 
   End With: '_dir' And The Colored Ones To End With: '_light'. Otherwise It Wouldn't Work!
   
   STN: The Script Is Incredibly Basic Yet Really Modular, You Can Put Pretty Much As
   Much Lightmaps As You Want And Change Between Them In A Single Line Of Code.
   MEANING: It Can Be Used Globally And Very Comfortably So!

   STN: Everything That The Script Needs ALREADY HERE Meaning - 
   If You Import It In To A New Project, It Works Perfectly Fine
   Considering It Doesn't Rely On Other Scripts. 
*/
