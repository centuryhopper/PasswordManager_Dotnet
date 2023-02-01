import React, { FC, SetStateAction } from 'react'

interface Props {
  searchTerm: string;
  setSearchTerm: (value: SetStateAction<string>) => void;
}

const SearchBar : FC<Props> = ({searchTerm, setSearchTerm}) : JSX.Element =>
{

  return (
    <input type="text" className="form-control" placeholder="Filter by title" onChange={e => {
        // console.log(searchTerm)
        setSearchTerm(e.target.value)
    }}/>
  )
}

export default SearchBar