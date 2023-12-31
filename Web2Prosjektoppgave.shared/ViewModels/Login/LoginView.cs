﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Web2Prosjektoppgave.shared.ViewModels.Login;

public class LoginView
{
    [Required]
    public string UserName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public bool RememberMe { get; set; }
}