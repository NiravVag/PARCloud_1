﻿
import React, { useEffect, useState } from 'react';
import { Link, useLocation } from "react-router-dom";
import PropTypes from "prop-types";
//// import "./assets/images/par_1.png";

//// react-bootstrap components
import {
    Badge,
    Button,
    ButtonGroup,
    Card,
    Collapse,
    Form,
    InputGroup,
    Navbar,
    Nav,
    Pagination,
    Container,
    Row,
    Col,
} from "react-bootstrap";


function Sidebar({ routes, image, background }) {
   

    const [state, setState] = React.useState({}); 
 

    //const [selectedTabValue, setSelectedTabValue] = useState(tabItems[3].value);
   // const handleTabItemClick = (event) => {
   //      let clickedTabValue = event.currentTarget.getAttribute("tab-value");
  //      setSelectedTabValue(clickedTabValue);
  //  }

    // to check for active links and opened collapses
    let location = useLocation();
    // this is for the user collapse
    const [userCollapseState, setUserCollapseState] = React.useState(false);
    // this is for the rest of the collapses
    
    React.useEffect(() => {
        setState(getCollapseStates(routes));
    }, []);
    // this creates the intial state of this component based on the collapse routes
    // that it gets through routes prop
    const getCollapseStates = (routes) => {
        let initialState = {};
        routes.map((prop, key) => {
            if (prop.collapse) {
                initialState = {
                    [prop.state]: getCollapseInitialState(prop.views),
                    ...getCollapseStates(prop.views),
                    ...initialState,
                };
            }
            return null;
        });
        return initialState;
    };
    // this verifies if any of the collapses should be default opened on a rerender of this component
    // for example, on the refresh of the page,
    // while on the src/views/forms/RegularForms.jsx - route /admin/regular-forms
    const getCollapseInitialState = (routes) => {
        for (let i = 0; i < routes.length; i++) {
            if (routes[i].collapse && getCollapseInitialState(routes[i].views)) {
                return true;
            } else if (location.pathname ===  routes[i].path) {
                return true;
            }
        }
        return false;
    };
    // this function creates the links and collapses that appear in the sidebar (left menu)
    const createLinks = (routes, subMenu) => {
        return routes.map((prop, key) => {
            if (prop.redirect) {
                return;
            }           
            if (prop.collapse) {
                var st = {};
                st[prop["state"]] = !state[prop.state];
                return (
                    <Nav.Item
                        className={getCollapseInitialState(prop.views) ? "active bg-white" : " bg-white"}
                        as="li"
                        key={key}
                    >
                        <Nav.Link
                            className={!state[prop.state] ? "collapsed" : ""}
                            data-toggle="collapse"
                            onClick={(e) => {
                                e.preventDefault();
                                setState({ ...state, ...st });
                            }}
                            aria-expanded={!state[prop.state]}
                        >
                            <i className={prop.icon}></i>
                            <p>
                                {prop.name} <b className="caret"></b>
                            </p>
                        </Nav.Link>
                        <Collapse className in={!state[prop.state]}>
                            <div>
                                <Nav className="bg-white" as="ul">{createLinks(prop.views, true)}</Nav>
                            </div>
                        </Collapse>
                    </Nav.Item>
                );
            }

            if (subMenu) {
                return (
                    <Nav.Item
                        className={activeRoute(prop.path)}
                        key={key}
                        as="li"
                    >
                        <Nav.Link className="d-flex align-items-center nav-link sub-menu" to={prop.path} as={Link}>
                            
                                <>

                                    <i className={prop.icon} />
                                    <p className="menu-name">{prop.name}</p>

                                </>
                            
                        </Nav.Link>
                    </Nav.Item>
                );

            }
            else {
                return (
                    <Nav.Item
                        className={activeRoute(prop.path)}
                        key={key}
                        as="li"
                    >
                        <Nav.Link className="d-flex align-items-center nav-link " to={prop.path} as={Link}>
                            {prop.icon ? (
                                <>

                                    <i className={prop.icon} />
                                    <p className="menu-name">{prop.name}</p>

                                </>
                            ) : (
                                <>
                                    <span className="sidebar-mini">{prop.mini}</span>
                                    <span className="sidebar-normal">{prop.name}</span>
                                </>
                            )}
                        </Nav.Link>
                    </Nav.Item>
                );
            }


        });
    };
    // verifies if routeName is the one active (in browser input)
    const activeRoute = (routeName) => {
        return location.pathname === routeName ? "active" : "";
    };
   
  
    return (
        <>
            <div className="sidebar bg-white" >
                <div className="sidebar-wrapper bg-white">
                    <div className="area">
                        <ul className="circles">
                            <li></li>
                            <li></li>
                            <li></li>
                            <li></li>
                            <li></li>
                            <li></li>
                            <li></li>
                            <li></li>
                            <li></li>
                            <li></li>
                        </ul>
                    </div>
                  
              
                    
                    <div className="logo">
                        { /*
                        <a className="simple-text logo-mini">
                            <div className="logo-img">
                                <img src="./assets/images/par_1.png"/>

                            </div>
                        </a>
                        */}
                        
                        
                    </div>                   
                    <Nav as="ul">{createLinks(routes, false)}</Nav>
                    
    
                </div>
                
            </div>
        </>
    );
}
//const [state, setstate] = useState({ data: "" })


let linkPropTypes = {
    path: PropTypes.string,
    layout: PropTypes.string,
    name: PropTypes.string,
    component: PropTypes.oneOfType([PropTypes.func, PropTypes.element]),
};

Sidebar.defaultProps = {
    image: "",
    background: "black",
    routes: [],
};

Sidebar.propTypes = {
    image: PropTypes.string,
    background: PropTypes.oneOf([
        "black",
        "azure",
        "green",
        "orange",
        "red",
        "purple",
    ]),
    routes: PropTypes.arrayOf(
        PropTypes.oneOfType([
            PropTypes.shape({
                ...linkPropTypes,
                icon: PropTypes.string,
            }),
            PropTypes.shape({                
                path: PropTypes.string,
                name: PropTypes.string,                
                icon: PropTypes.string,
                component: PropTypes.object                
            }),
        ])
    ),
};

export default Sidebar;
