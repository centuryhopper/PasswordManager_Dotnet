import React, { FC, useEffect, useState } from 'react';
import Button from 'react-bootstrap/Button';
import Form from 'react-bootstrap/Form';
import Modal from 'react-bootstrap/Modal';
import { AccountType, updateDialogType } from '../../interfaces/interfaces';

interface Props
{
    onDialog: (choose: boolean, updatedAccount: AccountType) => Promise<void>,
    handleDialog: (message: string, isLoading: boolean, account: AccountType) => void,
    dialog: updateDialogType,
    account: AccountType,
}

export const UpdateDialog : FC<Props> = ({onDialog, handleDialog, dialog, account}) : JSX.Element =>
{
    const [title, setTitle] = useState<string>('')
    const [username, setUserName] = useState<string>('')
    const [password, setPassword] = useState<string>('')

    // called right after component mounts to the virtual dom
    useEffect(() => {
        setTitle(account == undefined ? '' : account.title!)
        setUserName(account == undefined ? '' : account.username!)
        setPassword(account == undefined ? '' : account.password!)
    }, [account])

    return (
        <>
            <Modal show={dialog.isLoading} onHide={() => handleDialog('', false, {})}>
                <Modal.Header closeButton>
                <Modal.Title>{dialog.account.title}</Modal.Title>
                </Modal.Header>

                <Modal.Body>
                    {dialog.message}
                    {/* {JSON.stringify(account)} */}

                    <Form>
                        <Form.Group className="mb-3" controlId="ControlInput1">
                            <Form.Label>Title</Form.Label>
                            <Form.Control
                                type="text"
                                placeholder={`Old title is: ${title}. Please enter a new one...`}
                                onChange={e => setTitle(e.target.value)}
                                autoFocus
                                required
                            />
                        </Form.Group>

                        <Form.Group className="mb-3" controlId="ControlInput2">
                            <Form.Label>Username</Form.Label>
                            <Form.Control
                                type="text"
                                placeholder={`Old username is: ${username}. Please enter a new one...`}
                                onChange={e => setUserName(e.target.value)}
                                required
                            />
                        </Form.Group>

                        <Form.Group className="mb-3" controlId="ControlInput3">
                            <Form.Label>Password</Form.Label>
                            <Form.Control
                                as="textarea"
                                rows={5}
                                placeholder={`Old password is: ${password}. Please enter a new one...`}
                                onChange={e => setPassword(e.target.value)}
                                required
                            />
                        </Form.Group>
                    </Form>
                </Modal.Body>

                <Modal.Footer>
                    <Button variant="secondary" onClick={() => onDialog(false, {})}>
                        Close
                    </Button>
                    <Button type='submit' variant="primary" onClick={() => onDialog(true, {id: account.id, title, username, password})}>
                        Save Changes
                    </Button>
                </Modal.Footer>
            </Modal>
        </>
    )
}

