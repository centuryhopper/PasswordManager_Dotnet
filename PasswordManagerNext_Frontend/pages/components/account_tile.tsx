import React, { FC, SetStateAction, useState } from 'react'
import { AccountType } from '../../interfaces/interfaces';

interface Props
{
    lstOfAccounts: AccountType[];
    setLstOfAccounts: (value: SetStateAction<AccountType[]>) => void;
    account: AccountType
    handleDelete : (id: string) => void
    handleUpdate : (id: string) => void
}


export const AccountTitle : FC<Props> = ({account, lstOfAccounts, setLstOfAccounts, handleDelete, handleUpdate}) =>
{
  const [visible, setVisible] = useState<boolean>(false);

  return (
    <tr>
        <th scope="row">{account.id}</th>
        <td>{account.title}</td>
        <td>{account.username}</td>
        <td className='text-wrap' style={{wordBreak: 'break-all'}}>
          <input
            value={account.password}
            type={visible ? "text" : "password"}
            readOnly
          />
        </td>
        <td>
            <button
            type="submit"
            className="btn btn-primary ms-1"
            onClick={() => setVisible(!visible)}
            >
              Toggle
            </button>
            &nbsp;
            <button
            type="submit"
            className="btn btn-danger ms-1"
            onClick={() => handleDelete(account.id!)}
            >
              Delete
            </button>
            &nbsp;
            <button
            type="submit"
            className="btn btn-success"
            onClick={() => handleUpdate(account.id!)}
            >
              Update
            </button>
        </td>
    </tr>
  )
}
