import React, { useState } from 'react';

// react-bootstrap components
import {
    Badge,
    Button,
    Card,
    Form,
    InputGroup,
    Navbar,
    Nav,
    Table,
    Container,
    Row,
    Col,
    OverlayTrigger,
    Tooltip,
    ListGroup
} from "react-bootstrap";


const SearchBar = (props) => {
    const [searchText, setSearchText] = useState("");

    const handleSearchChange = (event) => {
        const value = event.target.value;
        setSearchText(value);
        props.onSearch(value);
    };

    return (
      
        <div className="mt-2 px-1">
            <div className="align-items-center input-group border rounded-pill bg-white">
                <div className="input-group-prepend border-0">
                    <button id="button-addon4" type="button" className="btn btn-link text-info"><i className="text-secondary fa fa-search"></i></button>
                </div>
                <input value={searchText} onChange={handleSearchChange} type="search" placeholder="Search" className="form-control bg-none border-0"/>
            </div>

           
            </div>

    );
}

export default SearchBar