import React from "react"
import './AppLetterBox.scss'

interface Props {
    letter: string,
    status: string
}

const AppLetterBox: React.FC<Props> = ({
    letter = "",
    status = "none"
}) => {
    return (
        <div
            className={`${status} box`}
            >
                {letter}
            </div>
    )
}

export default AppLetterBox