import React, { useEffect, useState } from "react"
import AppLetterBox from "../AppLetterBox/AppLetterBox"
import './AppGameBoard.scss'

interface Props {
    wordLength?: number,
    attempts?: number
}

class WordleWord {
    letters: WordleLetter[] = []

    toString = () : string => {
        var response = ""
        this.letters.forEach(w => {
            response += w.letter.toUpperCase()
        })
        return response
    }
}

class WordleLetter {
    letter: string = ""
    state: string = "none"
}

const AppGameBoard: React.FC<Props> = ({
    wordLength = 5,
    attempts = 5
}) => {

    const [words, setWords] = useState<WordleWord[]>([])
    const [activeWord, setActiveWord] = useState<string>("")

    useEffect(() => {
        document.addEventListener('keydown', keyPressed)
        return () => document.removeEventListener('keydown', keyPressed, false)
    })

    function keyPressed(event: any): void {
        var letter = event.key

        //Check for backspace
        if(event.key === "Backspace" || event.key === "Delete") {
            setActiveWord(activeWord.substring(0, activeWord.length - 1))
            return
        }
        else if(event.key === "Enter" && activeWord.length === wordLength) {
            
        }

        //Check if the key pressed is a letter.
        if(!letter.match(/[a-zA-Z]/i) || letter.length > 1) { return }

        if(activeWord.length == wordLength) { return; }

        var newWord = `${activeWord}${letter.toUpperCase()}`
        setActiveWord(newWord)
    }

    function getPlayField(): any {
        var rows = []
        for(var i = 0; i < attempts; i++) {
            rows.push(<div className="row" key={i}>{getRow(i)}</div>)
        }
        return rows
    }

    function getRow(rowIndex: number): any {

        var columns = []
        let word = words[rowIndex]

        for(var i = 0; i < wordLength; i++) {

            if(word !== undefined) {
                let letter = word.letters[i]
                columns.push(<AppLetterBox key={`${rowIndex}-${i}`} letter={letter.letter} status={letter.state} ></AppLetterBox>)
            }   
            else if(words.length === rowIndex) {
                let letter = activeWord.charAt(i)
                columns.push(<AppLetterBox key={`${rowIndex}-${i}`} letter={letter} status="none" ></AppLetterBox>)
            }
            else {
                columns.push(<AppLetterBox key={`${rowIndex}-${i}`} letter="" status="none" ></AppLetterBox>)
            }

        }
    
        return columns
    }


    return (
        <div className="board">
            {getPlayField()}
        </div>
    )
}

function makeWord(word: string): WordleWord {

    let finalWord = "HELLO"

    var response = new WordleWord()

    for(var i = 0; i < word.length; i++) {

        var state = "none"
        var letter = word.charAt(i).toUpperCase()

        if(finalWord.indexOf(letter) != -1) { state = "wrong-position" }
        if(finalWord.charAt(i).toUpperCase() === letter) { state = "correct" }

        response.letters.push({
            letter: letter,
            state: state
        })
    }

    return response

}






export default AppGameBoard