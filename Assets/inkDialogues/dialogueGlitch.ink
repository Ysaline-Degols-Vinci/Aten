
VAR coins = 0
EXTERNAL gainCoin(x)
VAR TrashCoin = false
EXTERNAL setTrashCoin(value)
VAR TossedCoinWell = false
EXTERNAL setTossedCoinWell(value)
EXTERNAL StartQuest(questID)
EXTERNAL AdvanceQuest(questID)
EXTERNAL FinishQuest(questID)
VAR CollectWoodQuestID = "CollectWood"
VAR CollectWoodState = "REQUIREMENTS_NOT_MET"
EXTERNAL AddItemInventory(string itemName)
EXTERNAL IsItemQuantityPresent(string itemName)
EXTERNAL RemoveFromInventory(string itemName, int quantity)



->Glitch

=== Glitch ===
    ... #speaker:Glitch#portrait:Glitch
    ... #speaker:Bubble#portrait:Bubble
    ... #speaker:Arlo#portrait:Arlo
    * [Try]
        Are you ok..? #speaker:Arlo#portrait:Arlo
        ... #speaker:Glitch#portrait:Glitch
        \*blub* #speaker:Bubble#portrait:Bubble
        Fine. #speaker:Arlo#portrait:ArloAngry
    -> DONE
    
    * [Leave]
        Yeah. Uh. I'm gonna go. #speaker:Arlo#portrait:Arlo
    -> DONE
    
=== Door ===
    Cute door #speaker:Arlo#portrait:Arlo
    * [Enter]
        Closed... #speaker:Arlo#portrait:Arlo
        Let. Me. In. #speaker:Arlo#portrait:ArloAngry
    -> DONE
    
    * [Leave]
        Anyway.
    -> DONE
    
    === Well ===
    A well! #speaker:Arlo#portrait:Arlo
    
    {coins > 0:
    * [Toss coin]
        I wish to go back home.. #speaker:Arlo#portrait:Arlo
        ~ gainCoin(-1)
        ~ setTossedCoinWell(true)
        
    -> DONE
    
    * [Leave]
        Anyway. #speaker:Arlo#portrait:Arlo
    -> DONE
    - else:
        This cat seems to be sleeping well. #speaker:Arlo#portrait:Arlo
     -> DONE
    }
    
    
  === Trash ===
{TrashCoin:
    No more coins.. #speaker:Arlo#portrait:Arlo
    -> DONE
- else:
    Oh? #speaker:Arlo#portrait:Arlo
    A coin. #speaker:Arlo#portrait:Arlo
    ~ gainCoin(1)
    ~ setTrashCoin(true)
    -> DONE
}
    
    === BlobCat ===
    { CollectWoodState :
        - "REQUIREMENTS_NOT_MET" : -> requirementsNotMet
        - "CAN_START" : -> canStart
        - "IN_PROGRESS" : -> inProgress
        - "CAN_FINISH" : -> canFinish
        - "FINISHED" : ->finished
        - else: -> END
    }
    = requirementsNotMet
    nuh uh not possible #speaker:Blob#portrait:BlobCat
    ->END
    
    = canStart
    \*blub* (I need a piece of wood!) #speaker:Blob#portrait:BlobCat
    \*blub?* (Could you get me one?) #speaker:Blob#portrait:BlobCat
    * [Yes]
    \*blub* (Great! Take this axe!) #speaker:Blob#portrait:BlobCat
    ~ AddItemInventory("Axe")
    ~ StartQuest(CollectWoodQuestID)
    ->DONE
    *[No]
    \*blub* (Aw man. I have no arms, I can't do this myself) #speaker:Blob#portrait:BlobCat
    -> DONE
    ->END
    
    = inProgress
    { IsItemQuantityPresent("Wood") >= 1:
    \*blub!* (Yess perfect!) #speaker:Blob#portrait:BlobCat
    \*blub* (take these coins) #speaker:Blob#portrait:BlobCat
    \*blub* (You can keep the axe! Can't use it anyway..) #speaker:Blob#portrait:BlobCat
    ~ AdvanceQuest(CollectWoodQuestID)
    ~ FinishQuest(CollectWoodQuestID)
    ~ RemoveFromInventory("Wood", 1)
    -> END
    - else:
    \*blub* (Take the axe and hit a tree!) #speaker:Blob#portrait:BlobCat
    -> END
    }
    
    =canFinish
    Not supposed to happen anymore! #speaker:Arlo#portrait:Arlo
    ->END
    
    =finished
    \*blub!* (Thanks for the help kid!) #speaker:Blob#portrait:BlobCat
    Yeah, of course #speaker:Arlo#portrait:Arlo
    ->END 
    
    === MadleenShop ===
    Greetings, Traveler! #speaker:Madleen#portrait:MadleenShop
    -> MadleenDiscussions
    
    ===MadleenDiscussions===
   I sell a lot of things if you're interested! #speaker:Madleen#portrait:MadleenShop
     + [How is your shop doing?]
        Pretty good. Well, uh. Lilith does buy almost all my stuff.. #speaker:Madleen#portrait:MadleenShop
        Not a lot of people are interested in manual work. #speaker:Madleen#portrait:MadleenShop
    -> MadleenDiscussions
    
    + [What do you sell?]
        Lot of things! Wood, some tools.. #speaker:Madleen#portrait:MadleenShop
        Well, actually, not so many things. #speaker:Madleen#portrait:MadleenShop
        But hey, it's useful! #speaker:Madleen#portrait:MadleenShop
        And i can buy your stuff too! #speaker:Madleen#portrait:MadleenShop
    -> MadleenDiscussions
    
    + [Leave]
        Come back soon! I'm always open. #speaker:Madleen#portrait:MadleenShop
    -> DONE
    
  

    