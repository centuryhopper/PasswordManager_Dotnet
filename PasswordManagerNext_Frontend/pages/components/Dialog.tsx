import React, { FC, useState } from 'react'
import { Button, Modal } from "react-bootstrap";
import { dialogType } from '../../interfaces/interfaces';

interface Props
{
    message: string,
    onDialog: (choose: boolean, id: string) => Promise<void>,
    handleDialog: (message: string, isLoading: boolean, title: string) => void,
    dialog: dialogType,
    title: string,
    id: string
}

export const Dialog : FC<Props> = ({message, onDialog, title, id, handleDialog, dialog}) : JSX.Element =>
{
    return (
      <>
        <Modal show={dialog.isLoading} onHide={() => handleDialog('', false, '')}>
          <Modal.Header closeButton>
            <Modal.Title>{title}</Modal.Title>
          </Modal.Header>
          <Modal.Body>
            {message}
          </Modal.Body>
          <Modal.Footer>
            <Button variant="secondary" onClick={() => onDialog(false, id)}>
              No
            </Button>
            <Button variant="primary" onClick={() => onDialog(true, id)}>
              Yes
            </Button>
          </Modal.Footer>
        </Modal>
      </>
    );
}
