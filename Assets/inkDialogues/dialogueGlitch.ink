
VAR coins = 0
EXTERNAL gainCoin(x)
VAR TrashCoin = false
EXTERNAL setTrashCoin(value)
VAR TossedCoinWell = false
EXTERNAL setTossedCoinWell(value)



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
    
  

    