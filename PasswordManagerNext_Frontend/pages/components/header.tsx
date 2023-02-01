import React, { FC } from 'react'

const Header : FC<{title: string}> = ({title}) : JSX.Element => {
    return (
        <>
        <header>
            <h1 className="text-center">
                {title}
            </h1>
        </header>
        </>
    )
}

export default Header
