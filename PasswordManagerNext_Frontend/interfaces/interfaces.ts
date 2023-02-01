
export interface AccountType
{
    id?: string,
    title?: string,
    username?: string,
    password?: string
}

export type dialogType = {message:string, isLoading: boolean, title: string}

export type updateDialogType = {message:string, isLoading: boolean, account: AccountType}

