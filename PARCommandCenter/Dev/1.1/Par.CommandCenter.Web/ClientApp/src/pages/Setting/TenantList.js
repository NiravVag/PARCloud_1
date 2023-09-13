import React, { useState, useEffect } from 'react';

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



const TenantList = (props) => {
    
    let tenantItems;

    if (props.tenants) {
        tenantItems = props.tenants.map((tenant, index) => {
            let id = `${tenant.id}`;
            if (props.idPrefix && props.idPrefix.length > 0) {
              id = `${props.idPrefix}${tenant.id}`;
            }

            return (
                <tr key={index}>
                   
                    <td>
                        <Form.Check
                            type="switch"
                            id={id}
                            className="mb-1"
                            checked={tenant.selected}
                            onChange={props.onTenantToggleChange}
                        />
                    </td>

                    <td>{tenant.id}</td>
                    <td>{tenant.name}</td>
                    <td>{tenant.timeZone.name}</td>
                </tr>
            );
        });
    }

    return (
        <Table className="table-striped">
            <thead>
                <tr className="bg-white z-1000">
                    <th className="col-1 text-16">Selected</th>
                    <th className="text-16">Id</th>
                    <th className="text-16">Name</th>
                    <th className="text-16">Time Zone</th>
                </tr>
            </thead>
            <tbody>
                {tenantItems}
            </tbody>
        </Table>
    );

}

export default TenantList;