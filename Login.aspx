<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SO_Appraisal.Login" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Login - SYNAPSE</title>

    <!-- Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />

    <!-- Icons -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons/font/bootstrap-icons.css" rel="stylesheet" />

    <meta name="viewport" content="width=device-width, initial-scale=1" />

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

        body {
            background-color: #ffffff;
            min-height: 100vh;
            margin: 0;
        }

        .login-wrapper {
            min-height: 100vh;
            display: flex;
            justify-content: center;
            align-items: center;
            padding: 20px;
        }

        .login-card {
            width: 100%;
            max-width: 400px;
            padding: 30px 25px;
            border-radius: 8px;
            background: #ffffff;
            box-shadow: 0 8px 25px rgba(0,0,0,0.08);
            transition: 0.3s;
        }

            .login-card:hover {
                transform: translateY(-3px);
            }

        .login-title {
            font-weight: 600;
            text-align: center;
            margin-bottom: 25px;
            color: #333;
        }

        .input-wrapper {
            position: relative;
        }

            .input-wrapper i {
                position: absolute;
                top: 50%;
                left: 12px;
                transform: translateY(-50%);
                color: #6c757d;
            }

        .form-control {
            /*border-radius: 10px;*/
            padding-left: 40px;
            height: 45px;
        }

            .form-control:focus {
                box-shadow: none;
                border-color: #0d6efd;
            }

        .btn-login {
            border-radius: 4px;
            font-weight: 500;
            height: 45px;
        }

        .footer-text {
            text-align: center;
            font-size: 12px;
            margin-top: 15px;
            color: #888;
        }

        /* 📱 Mobile optimization */
        @media (max-width: 576px) {
            .login-card {
                padding: 25px 20px;
                border-radius: 8px;
            }

            .login-title {
                font-size: 20px;
            }
        }

        .login-logo {
            max-width: 200px;
            height: auto;
        }
    </style>

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.bundle.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.29.1/moment.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.9.0/js/bootstrap-datepicker.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/html2canvas/1.4.1/html2canvas.min.js"></script>
    <script src="https://code.jquery.com/ui/1.13.2/jquery-ui.min.js"></script>

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

        function showNoteAlert() {
            $('#noteAlert').show();
        }

        function hideNoteAlert() {
            $('#noteAlert').hide();
        }
    </script>
</head>

<body>
    <form id="form1" runat="server">
        <div class="login-wrapper">

            <div class="login-card">

                <div class="text-center mb-3">
                    <img src="Images/logo.png" alt="SYNAPSE"
                        class="login-logo" />
                </div>

                <!-- Username -->
                <div class="mb-3 input-wrapper">
                    <i class="bi bi-person"></i>
                    <asp:TextBox ID="txtUsername" runat="server"
                        CssClass="form-control"
                        Placeholder="UserId"></asp:TextBox>
                </div>

                <!-- Password -->
                <div class="mb-3 input-wrapper">
                    <i class="bi bi-lock"></i>
                    <asp:TextBox ID="txtPassword" runat="server"
                        CssClass="form-control"
                        TextMode="Password"
                        Placeholder="Password"></asp:TextBox>
                </div>

                <!-- Login Button -->
                <div class="d-grid">
                    <asp:Button ID="btnLogin" runat="server"
                        Text="Login"
                        CssClass="btn btn-primary btn-login"
                        OnClick="btnLogin_Click" />
                </div>

                <!-- Footer -->
                <div class="footer-text">
                    © <%: DateTime.Now.Year %> SYNAPSE
                </div>

            </div>

        </div>

         <div id="toastContainer" aria-live="polite" aria-atomic="true" style="position: relative; min-height: 200px;"></div>
    </form>
</body>
</html>
