import React from "react";
import './AppButton.scss'

interface Props {
    size?: string,
    children?: React.ReactNode,
    onClick: () => void
}

const AppButton: React.FC<Props> = ({
    size = "medium",
    children,
    onClick
}) => {
    return (
        <button
            onClick={onClick}
            className={size}
            style={{
            }}
        >
            {children}
        </button>
    )
}

export default AppButton