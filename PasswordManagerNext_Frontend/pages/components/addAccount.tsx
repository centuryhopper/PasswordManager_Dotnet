import React, { FC, SetStateAction, useReducer, useRef} from 'react';
import { AccountType } from '../../interfaces/interfaces';
import { v4 as uuidv4 } from 'uuid';
import {URL} from '../../constants/constants'

interface Props
{
    lstOfAccounts: AccountType[];
    setLstOfAccounts: (value: SetStateAction<AccountType[]>) => void;
}

const AddAccount : FC<Props> = ({lstOfAccounts, setLstOfAccounts}) : JSX.Element =>
{
    const handleAdd = async (event : React.FormEvent<HTMLFormElement>) : Promise<void> =>
    {
        event.preventDefault()

        console.log('adding account to database');

        const accountToPost = {
            id: uuidv4(),
            title: addAccountState.title,
            username: addAccountState.username,
            password: addAccountState.password
        }

        try {
            // make post request and update overall list of accounts in the table
            const response = await fetch(URL, {
                method: 'POST', // or 'PUT'
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(accountToPost),
            })


            const result = await response.json()

            console.log(result);

            setLstOfAccounts([...lstOfAccounts, accountToPost])

            // clear inputs
            addAccountDispatch({type: 'TITLE', payload: ''})
            addAccountDispatch({type: 'USERNAME', payload: ''})
            addAccountDispatch({type: 'PASSWORD', payload: ''})

            if (titleInputRef !== null && titleInputRef.current !== null)
                titleInputRef.current.value = ''
            if (usernameInputRef !== null && usernameInputRef.current !== null)
                usernameInputRef.current.value = ''
            if (passwordInputRef !== null && passwordInputRef.current !== null)
                passwordInputRef.current.value = ''

        } catch (error: any) {
            console.log(error.message)
        }
    }

    const [addAccountState, addAccountDispatch] = useReducer((state:any, action:any) => {
        switch (action.type)
        {
            case 'TITLE':
                return {title: action.payload, username: state.username, password: state.password}
            case 'USERNAME':
                return {title: state.title, username: action.payload, password: state.password}
            case 'PASSWORD':
                return {title: state.title, username: state.username, password: action.payload}
            default:
                return state
        }

    }, {title: '', username: '', passsword: ''})

    const titleInputRef = useRef<HTMLInputElement>(null)
    const usernameInputRef = useRef<HTMLInputElement>(null)
    const passwordInputRef = useRef<HTMLInputElement>(null)

    return (
        <>
            {/* <input type="text" className="form-control" placeholder="Add an account" onChange={e => { */}
                {/* // console.log(searchTerm)
                // setSearchTerm(e.target.value)
            }}/> */}

            <form onSubmit={handleAdd}>
                <div className="input-group">
                    {/* <span className="input-group-text">First and last name</span> */}
                    <input ref={titleInputRef} type="text" aria-label="title" className="form-control" placeholder="Enter a title..." onChange={e => addAccountDispatch({type: 'TITLE', payload: e.target.value})} required/>
                    <input ref={usernameInputRef} type="text" aria-label="username" className="form-control" placeholder="Enter a username..." onChange={e => addAccountDispatch({type: 'USERNAME', payload: e.target.value})} required/>
                    <input ref={passwordInputRef} type="text" aria-label="password" className="form-control" placeholder="Enter a password..." onChange={e => addAccountDispatch({type: 'PASSWORD', payload: e.target.value})} required/>
                </div>
                <button
                    type="submit"
                    className="btn btn-primary ms-1">
                    Add account
                </button>
            </form>

        {/* debugging */}
            {/* <p>{addAccountState.title}</p>
            <p>{addAccountState.username}</p>
            <p>{addAccountState.password}</p> */}



        </>
    )
}

export default AddAccount