import { FC, useRef, useState } from 'react'
import { AccountType, dialogType, updateDialogType} from '../../interfaces/interfaces'
import {AccountTitle} from './account_tile'
import SearchBar from './search_bar'
import useSWR from 'swr'
import AddAccount from './addAccount'
import { Dialog } from './Dialog'
import {URL} from '../../constants/constants'
import { UpdateDialog } from './UpdateDialog'


const TableView : FC = () =>
{
    const [searchTerm, setSearchTerm] = useState<string>('')

    const [lstOfAccounts, setLstOfAccounts] = useState<AccountType[]>([])
    const idProductRef = useRef<string>();
    const accountRef = useRef<AccountType>();

    const { data, error } = useSWR(URL, async (url : string) =>
    {
        const res = await fetch(url)

        const data = await res.json()

        setLstOfAccounts(JSON.parse(data))

        // for (const acc of lstOfAccounts)
        // {
        //     console.log(acc);
        // }

        return data
    })

    const [deleteDialog, setDeleteDialog] = useState<dialogType>({
        message: "",
        isLoading: false,
        title: ""
    })

    const [updateDialog, setUpdateDialog] = useState<updateDialogType>({
        message: "",
        isLoading: false,
        account: {id: '', title: '', username: '', password: ''}
    })

    const handleDelete = (id: string): void =>
    {
        console.log('deleting account from database')

        // confirm deletion by asking the user again

        const index = lstOfAccounts.findIndex((p) => p.id === id);

        handleDeleteDialog("Are you sure you want to delete?", true, lstOfAccounts[index].title!)

        idProductRef.current = id;
    }

    const handleDeleteDialog = (message:string, isLoading:boolean, title:string) : void => {
        setDeleteDialog({
            message,
            isLoading,
            //Update
            title
        })
    }

    const areUSureDelete = async (choose: boolean, id: string) : Promise<void> => {
        if (choose)
        {
            setLstOfAccounts(lstOfAccounts.filter((p) => p.id !== id));

            try {
                const response = await fetch(`${URL}${id}`, {method: 'DELETE'})

                const result = await response.json()

                console.log(result);
            } catch (error : any) {
                console.log(error.message)
            }

            // close dialog
            handleDeleteDialog("", false, '');
        }
        else
        {
            // close dialog
            handleDeleteDialog("", false, '');
        }
    }

    const handleUpdate = (id: string): void =>
    {
        // put request
        console.log('updated account')

        const index = lstOfAccounts.findIndex((p) => p.id === id);

        handleUpdateDialog("Are you sure you want to update?", true, lstOfAccounts[index]!)

        accountRef.current = lstOfAccounts[index]

        // grab the state of the lstOfAccounts on component mount (i.e. use useEffect(()=>{}, []))
        // if any fields have changed, then ask the user if he/she wants to confirm the update. If so, then update the list
        // and the database. Otherwise, revert the changes made from the current account selected
    }

    const areUSureUpdate = async (choose: boolean, updatedAccount: AccountType) : Promise<void> => {
        if (choose)
        {
            // find existing account by id
            const index = lstOfAccounts.findIndex((p) => p.id === updatedAccount.id);

            // replace its values with that of the updatedAccount
            setLstOfAccounts((prevLst : AccountType[]) => {
                prevLst[index].title = updatedAccount.title
                prevLst[index].username = updatedAccount.username
                prevLst[index].password = updatedAccount.password
                return prevLst
            })

            try {
                const response = await fetch(URL,  {
                    method: 'PUT',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(updatedAccount)
                })
                const result = await response.json()

                console.log(result);
            } catch (error : any) {
                console.log(error.message)
            }

            // close dialog
            handleUpdateDialog("", false, {});
        }
        else
        {
            // close dialog
            handleUpdateDialog("", false, {});
        }
    }

    const handleUpdateDialog = (message: string, isLoading: boolean, account: AccountType) : void => {
        setUpdateDialog({
            message,
            isLoading,
            //Update
            account
        })
    }

    if (error)
    {
        console.log(error)
        return <div>Failed to load</div>
    }
    if (!data)
    {
        return <div>Loading...</div>
    }

    // console.log(data)
    // console.log('hello')
    // console.log(error)

    return (
        <section className="vh-100" style={{backgroundColor: "#eee"}}>
            <div className="container py-5 h-100">
                <div className="row d-flex justify-content-center align-items-center h-100">
                <div className="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12">
                    <div className="card rounded-3">
                    <div className="card-body p-4">
                        <h4 className="text-center my-3 pb-3">Password database query</h4>
                        <div className="col-12">
                            <div className="form-outline">
                                <SearchBar searchTerm={searchTerm} setSearchTerm={setSearchTerm}/>
                                <AddAccount lstOfAccounts={lstOfAccounts} setLstOfAccounts={setLstOfAccounts}/>
                            </div>
                        </div>

                        <div className="table-responsive">
                            <table className="table mb-4">
                            <thead>
                                <tr>
                                    <th scope="col">ID</th>
                                    <th scope="col">Title</th>
                                    <th scope="col">Username</th>
                                    <th scope="col">Password</th>
                                    <th scope="col">Actions</th>
                                </tr>
                            </thead>
                            <tbody>
                                {
                                    lstOfAccounts
                                    .filter((account: AccountType) => searchTerm === '' || searchTerm.toLowerCase().includes(account.title!))
                                    .map((account : AccountType) =>
                                        {
                                            return (<AccountTitle
                                                key={account.id}
                                                account={account}
                                                lstOfAccounts={lstOfAccounts}
                                                setLstOfAccounts={setLstOfAccounts}
                                                handleDelete={handleDelete}
                                                handleUpdate={handleUpdate}
                                            />)
                                        }
                                    )
                                }
                            </tbody>
                            </table>
                        </div>

                        <Dialog
                            id={idProductRef.current!}
                            title={deleteDialog.title}
                            onDialog={areUSureDelete}
                            message={deleteDialog.message}
                            dialog={deleteDialog}
                            handleDialog={handleDeleteDialog}
                        />

                        <UpdateDialog
                            onDialog={areUSureUpdate}
                            dialog={updateDialog}
                            handleDialog={handleUpdateDialog}
                            account={accountRef.current!}
                        />
                    </div>
                    </div>
                </div>
                </div>
            </div>
        </section>
    )
}

export default TableView

