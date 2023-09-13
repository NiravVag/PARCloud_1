import React, { useEffect, useState } from 'react';

import ReactTable from './../../../components/Common/ReactTable/ReactTable';

import {
    Badge,
    Button,
    ButtonGroup,
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
    ListGroup,
    Modal,
    FormControl,
    Spinner
} from "react-bootstrap";

const FacilitesPanel = (props) => {
    const [isLoading, setIsLoading] = useState(true);
    const [facilities, setFacilities] = useState([]);

    let facilitiesTable = undefined;
    let data = [];

    const url = "api/Facility/GetByTenantId/" + props.tenant.id;

    useEffect(() => {
        const fetchData = async (url) => {
            try {
                setIsLoading(true);

                const response = await fetch(url);

                if (response.status === 204) {
                    setFacilities([]);
                } else {
                    const data = await response.json();
                    setFacilities(data.facilities);
                }

                setIsLoading(false);

            } catch (error) {
                setIsLoading(false);
                throw error;
            }
        };

        fetchData(url);
    }, [url, props.onRefresh]);    

    if (facilities && facilities.length > 0) {
        data = facilities.map((prop, key) => {
            return {
                id: prop.id,
                name: prop.name,
                vpnConnectionName: prop.vpnConnectionName,
                addressLine1: prop.addressLine1,
                city: prop.city,
                state: (prop.state) ? prop.state.name : "",
                postalCode: prop.postalCode,                
                actions: (
                    <div className="actions-right">
                        <Button
                            onClick={() => {
                                let obj = facilities.find((o) => o.id === prop.id);
                                props.onEditFacility(obj);
                            }
                            }
                            variant="warning"
                            size="sm"
                            className="text-warning btn-link edit"
                        >
                            <i className="fa fa-edit" />
                        </Button>{" "}
                        {/*<Button*/}
                        {/*    variant="danger"*/}
                        {/*    size="sm"*/}
                        {/*    className="btn-link remove text-danger"*/}
                        {/*>*/}
                        {/*    <i className="fa fa-times" />*/}
                        {/*</Button>{" "}*/}
                    </div>
                ),
            };
        });
    }

    facilitiesTable = (
        <ReactTable
            data={data}
            columns={[
                {
                    Header: "Name",
                    accessor: "name",
                    className: 'rt-custom',
                    maxWidth: 150,
                    minWidth: 150,
                    width: 150,
                },
                {
                    Header: "VPN Connection Name",
                    accessor: "vpnConnectionName",
                    className: 'rt-custom',
                    maxWidth: 140,
                    minWidth: 140,
                    width: 140,
                },
                {
                    Header: "Address",
                    accessor: "addressLine1",
                    className: 'rt-custom',
                    maxWidth: 140,
                    minWidth: 140,
                    width: 140,
                },
                {
                    Header: "City",
                    accessor: "city",
                    className: 'rt-custom',
                    maxWidth: 100,
                    minWidth: 80,
                    width: 100,
                },
                {
                    Header: "State",
                    accessor: "state",
                    className: 'rt-custom',
                    maxWidth: 70,
                    minWidth: 70,

                    width: 70,
                },
                {
                    Header: "Postal Code",
                    accessor: "postalCode",
                    className: 'rt-custom',
                    maxWidth: 100,
                    minWidth: 80,
                    width: 80,
                },
                {
                    Header: "Actions",
                    accessor: "actions",
                    className: 'rt-custom',
                    sortable: false,
                    filterable: false,
                    maxWidth: 70,
                    minWidth: 70,

                    width: 70,
                },
            ]}
            /*
                You can choose between primary-pagination, info-pagination, success-pagination, warning-pagination, danger-pagination or none - which will make the pagination buttons gray
            */
            className="-striped -highlight primary-pagination"
        />
    );

    return (
        <>
            <Card.Body className="table-full-width">
                <Row>
                    <Col className="mx-auto setup-facilities-table pt-2">
                        {facilitiesTable}
                    </Col>
                </Row>
            </Card.Body>
        </>
    );
}

export default FacilitesPanel;
