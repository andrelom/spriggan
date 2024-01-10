function Set-EnvironmentVariables {
  [CmdletBinding(SupportsShouldProcess = $true, ConfirmImpact = 'Low')]
  param($file = ".\.env.local")

  if (!(Test-Path $file)) {
    Throw "Could not open $file"
  }

  $content = Get-Content $file -ErrorAction Stop

  Write-Verbose "Parsed .env.local file"

  foreach ($line in $content) {
    if ($line.StartsWith("#")) {
      continue
    }

    if ($line.Trim()) {
      $line = $line.Replace("`"", "")
      $kvp = $line -split "=", 2

      if ($PSCmdlet.ShouldProcess("$($kvp[0])", "set value $($kvp[1])")) {
        [System.Environment]::SetEnvironmentVariable($kvp[0].Trim(), $kvp[1].Trim(), "Process") | Out-Null
      }
    }
  }
}

Export-ModuleMember -Function @('Set-EnvironmentVariables')
