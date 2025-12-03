<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SO_Appraisal._Default" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.9.0/css/bootstrap-datepicker.min.css" />
    <link href="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/css/select2.min.css" rel="stylesheet" />

    <style>
        .toast-custom {
            position: fixed;
            top: 10px;
            right: 20px;
            width: 300px;
            background-color: #fff;
            border-radius: 5px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
            padding: 15px;
        }

        .toast-success {
            border-left: 5px solid #28a745; /* Light green */
        }

        .toast-danger {
            border-left: 5px solid #dc3545; /* Red */
        }

        .form-control {
            text-align: center;
        }

        @media (max-width: 768px) {
            .col-12 {
                text-align: center;
            }

            #btnOpenModal {
                width: 100%;
            }
        }

        .navbar-white .navbar-toggler {
            border-color: rgba(0, 0, 0, 0.1);
        }

        .navbar-white .navbar-toggler-icon {
            background-image: url("data:image/svg+xml;charset=utf8,%3Csvg viewBox='0 0 30 30' xmlns='http://www.w3.org/2000/svg'%3E%3Cpath stroke='rgba(0, 0, 0, 0.5)' stroke-width='2' stroke-linecap='round' stroke-miterlimit='10' d='M4 7h22M4 15h22M4 23h22'/%3E%3C/svg%3E");
        }

        /* Container for the progress bar */
        .progress-bar-container {
            width: 100%; /* Makes the container take up the full width */
            /*max-width: 600px;*/ /* Optional: sets a maximum width for larger screens */
            background-color: #e0e0e0;
            border-radius: 0px;
            overflow: hidden;
            position: relative;
            margin-top: -20px; /* Adjusts spacing from the <hr /> */
            margin-bottom: 20px; /* Optional: adds some space after the progress bar */
        }

        /* The animated progress bar */
        .progress-bar {
            height: 5px;
            background: linear-gradient(to right, #4caf50, #81c784, #4caf50); /* Green gradient */
            width: 0%; /* Starts from 0% width */
            animation: progress-animation 2s infinite; /* Animation for progress effect */
        }

        /* Keyframes for progress animation */
        @keyframes progress-animation {
            0% {
                width: 0%;
            }

            50% {
                width: 50%;
            }

            100% {
                width: 100%;
            }
        }

        /* Adjustments for small screens */
        @media (max-width: 768px) {
            .progress-bar-container {
                width: 100%; /* Full width on smaller screens */
                max-width: 100%; /* Ensures the progress bar can stretch to the screen size */
            }
        }

        /* Common bubble style */
        .bubble {
            position: absolute;
            border-radius: 50%;
            z-index: 0;
            opacity: 0.3;
        }

        /* Top-left bubble - 70% visible */
        .bubble-top-left {
            width: 300px;
            height: 300px;
            background-color: #8e44ad; /* Purple */
            top: -100px;
            left: -100px;
        }

        /* Bottom-center bubble - half hidden, slightly larger */
        .bubble-bottom-center {
            width: 400px;
            height: 400px;
            background-color: #6c5ce7; /* Violet */
            bottom: -200px;
            left: 33%;
            transform: translateX(-50%);
            z-index: 2;
            opacity: 0.5;
        }

        .person-image-wrapper {
            position: relative; /* Add this */
            display: flex;
            justify-content: flex-end;
            margin-top: -7px;
            margin-right: -92px;
            z-index: 1; /* Lower than bubble */
        }

        .person-image {
            max-width: 650px;
            height: auto;
            border-bottom-left-radius: 20%;
        }

        .welcome-text-wrapper {
            position: absolute;
            top: 20%;
            left: 5%;
            width: 45%;
            z-index: 3;
            color: #333;
        }

            .welcome-text-wrapper h1 {
                font-size: 4rem;
                font-weight: bold;
            }

            .welcome-text-wrapper h6 {
                font-size: 1rem;
                margin-top: 10px;
            }
    </style>

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.bundle.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.29.1/moment.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.9.0/js/bootstrap-datepicker.min.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>
    <script>
        function showToast(message, styleClass) {
            var toast = $('<div class="toast-custom ' + styleClass + '">' + message + '</div>').appendTo('#toastContainer');

            // Show the toast
            toast.fadeIn();

            // Move existing toasts down
            $('.toast-custom').not(toast).each(function () {
                $(this).animate({ top: "+=" + (toast.outerHeight() + 10) }, 'fast');
            });

            // Hide the toast after 3 seconds
            setTimeout(function () {
                toast.fadeOut(function () {
                    // Remove the toast from DOM after fadeOut
                    $(this).remove();

                    // Move remaining toasts up
                    $('.toast-custom').each(function (index) {
                        $(this).animate({ top: "-=" + (toast.outerHeight() + 10) }, 'fast');
                    });
                });
            }, 3000);
        }
    </script>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
 <%--   <div class="bubble bubble-top-left"></div>
    <div class="bubble bubble-bottom-center"></div>--%>





    <main aria-labelledby="title">
        <div class="container body-content">

            <div class="person-image-wrapper">
                <img src="" alt="person" class="person-image" />
            </div>

            <div class="welcome-text-wrapper">
                <h1>Welcome to the SO Appraisal Portal</h1>
                <br />
                <hr />
                <h6>Your gateway to streamlined operations, insights, and smarter decisions.</h6>
            </div>


            <%-- Notification Label --%>
            <div id="toastContainer" aria-live="polite" aria-atomic="true" style="position: relative; min-height: 200px;"></div>
            <asp:HiddenField ID="hdnBusinessType" runat="server" />
            <asp:HiddenField ID="hdnRole" runat="server" />
        </div>
    </main>
</asp:Content>

