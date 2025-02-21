# InstallationValidator

Installation Validation Tool for the Open Systems Pharmacology Suite.

## Code Status
[![Build status](https://img.shields.io/github/actions/workflow/status/Open-Systems-Pharmacology/InstallationValidator/build-and-test.yml?logo=nuget&label=Build%20status)](https://github.com/Open-Systems-Pharmacology/InstallationValidator/actions/workflows/build-and-test.yml)

## Building WIX output for setup
The following command should be run to create the `BatchFiles.wxs`
```
rake "heat[C:\\projects\\InstallationValidator\\data, BatchFiles]"
```

then in `BatchFiles.wxs` file:

```
* replace `$(var.DeployDir)` with `$(var.DeployDir)\..\..\data`
* remove entry for BatchFiles.wxs
* change INSTALLDIR to InstallationValidatorCommonDataVersionFolder
```

## Code of conduct
Everyone interacting in the Open Systems Pharmacology community (codebases, issue trackers, chat rooms, mailing lists etc...) is expected to follow the Open Systems Pharmacology [code of conduct](https://github.com/Open-Systems-Pharmacology/Suite/blob/master/CODE_OF_CONDUCT.md).

## Contribution
We encourage contribution to the Open Systems Pharmacology community. Before getting started please read the [contribution guidelines](https://github.com/Open-Systems-Pharmacology/Suite/blob/master/CONTRIBUTING.md). If you are contributing code, please be familiar with the [coding standards](https://github.com/Open-Systems-Pharmacology/Suite/blob/master/CODING_STANDARDS.md).

## License
InstallationValidator is released under the [GPLv2 License](https://github.com/Open-Systems-Pharmacology/Suite/blob/master/LICENSE).
